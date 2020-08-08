using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public BoidGroup group;
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    protected Vector3 goalPos = Vector3.zero;
    protected Vector3 currentForce;
    protected float minArriveDist = 3000f;
    protected float minArriveDistSq;
    protected float wanderTheta = 0;
    protected float maxVelocity;
    protected float maxForce;

    // Use this for initialization
    protected void Start()
    {

        location = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        velocity = new Vector3();
        acceleration = new Vector3();

        minArriveDistSq = minArriveDist * minArriveDist;
    }

    // Update is called once per frame
    protected void Update()
    {
        maxVelocity = group.maxVelocity;
        maxForce = group.maxForce;

        applyForce(currentForce);

        velocity += acceleration;
        location += velocity;
        acceleration *= 0;

        transform.position = location;
    }


    public Vector3 seek(Vector3 target)
    {
        Vector3 desired = target - location;
        desired = desired.normalized;
        desired *= maxVelocity;

        return steer(desired);
    }

    public Vector3 arrive(Vector3 target)
    {
        Vector3 desired = target - location;
        float d = desired.sqrMagnitude;
        desired = desired.normalized;
        if (d < minArriveDistSq)
        {
            float m = (d / minArriveDist) * maxVelocity;
            desired *= m;

            if (Random.value < group.spinProbability)
            {
                desired = rotate(desired, group.spinForce);
            }
        }
        else
        {
            desired *= maxVelocity;
        }

        return steer(desired);
    }

    public Vector3 rotate(Vector3 force, float intensity)
    {
        Vector3 rotated = new Vector3(-force.z, force.y, force.x);
        rotated = rotated.normalized * intensity;
        rotated *= maxVelocity;
        return rotated;
    }


    public Vector3 wander()
    {
        float wanderR = 25 * 0.002f;
        float wanderD = 80 * 0.002f;
        float change = 0.3f * 0.002f;
        wanderTheta += Random.Range(-change, change);

        Vector3 circlePos = new Vector3(velocity.x, velocity.y, velocity.z);
        circlePos = circlePos.normalized;
        circlePos *= wanderD;
        circlePos += location;

        float h = Mathf.Atan2(velocity.y, velocity.x);

        Vector3 circleOffset = new Vector3(wanderR * Mathf.Cos(wanderTheta + h), wanderR * Mathf.Sin(wanderTheta + h), 0);
        Vector3 target = circlePos + circleOffset;
        return seek(target);
    }

    public Vector3 checkEdges()
    {
        float d = 0.1f;
        Vector3 desired = Vector3.zero;
        bool hit = false;
        if (location.x < -5 + d)
        {
            desired = new Vector3(maxVelocity, velocity.y, velocity.z);
            hit = true;
        }
        else if (location.x > 5 - d)
        {
            desired = new Vector3(-maxVelocity, velocity.y, velocity.z);
            hit = true;
        }

        if (location.y < d)
        {
            desired = new Vector3(velocity.x, maxVelocity, velocity.z);
            hit = true;
        }
        else if (location.y > 2 - d)
        {
            desired = new Vector3(velocity.x, -maxVelocity, velocity.z);
            hit = true;
        }

        if (hit)
        {
            desired = desired.normalized;
            desired *= (maxVelocity * 50);

            return steer(desired);

        }

        return desired;

    }

    public Vector3 separate()
    {

        float desiredSeparation = 0.15f;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Boid other in group.boids)
        {
            if (!other || other == this) continue;

            float d = Vector3.Distance(location, other.location);
            if (d > 0 && d < desiredSeparation)
            {
                Vector3 diff = location - other.location;
                diff = diff.normalized;
                diff /= d;
                sum += diff;
                count++;
            }
            if (count > 0)
            {
                sum /= count;
                sum = sum.normalized;
                sum *= maxVelocity;

                return steer(sum);
            }
        }

        return Vector3.zero;
    }
    public Vector3 align()
    {
        float neighbordist = group.neighbourDistance;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Boid other in group.boids)
        {
            if (!other || other == this) continue;

            float d = Vector3.Distance(location, other.location);
            if (d > 0 && d < neighbordist)
            {
                sum += other.velocity;
                count++;
            }
            if (count > 0)
            {
                sum /= count;
                sum = sum.normalized;
                sum *= maxVelocity;
                return steer(sum);
            }
        }

        return Vector3.zero;
    }

    public Vector3 cohesion()
    {
        float neighbordist = group.neighbourDistance;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (Boid other in group.boids)
        {
            if (!other || other == this) continue;

            float d = Vector3.Distance(location, other.location);
            if (d > 0 && d < neighbordist)
            {
                sum += other.location;
                count++;
            }
            if (count > 0)
            {
                sum /= count;
                return seek(sum);
            }
        }

        return Vector3.zero;
    }

    public Vector3 flock()
    {
        return align() + cohesion() + separate();
    }

    public Vector3 steer(Vector3 target)
    {
        Vector3 steer = target - velocity;
        if (steer.magnitude > maxForce)
        {
            steer = steer.normalized;
            steer *= maxForce;
        }

        return steer;
    }

    public void applyForce(Vector3 force)
    {
        acceleration += force;
    }
}
