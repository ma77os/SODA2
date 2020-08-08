using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidGroup : MonoBehaviour
{
    public int maxBoids = 10;
    public GameObject boidPrefab;
    public Boid[] boids;
    
    public float neighbourDistance = 0.2f;
    
    public float maxForce = 0.0004f;
    
    public float maxVelocity = 0.01f;
    
    public float wanderForce = 3;
    
    public float wanderProbability = 30;

    
    public float spinForce = 1f;
    public float spinProbability = 70;

    protected int count = 0;


    // Use this for initialization
    protected void Start()
    {

        boids = new Boid[maxBoids];
        
    }

    protected void Update()
    {
        
    }

    protected void createBoid(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (count > maxBoids - 1) return;

            Vector3 initPos = new Vector3();
            //Vector3 initPos = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
            GameObject f = Instantiate(boidPrefab, this.transform.position + initPos, Quaternion.identity, transform) as GameObject;
            Boid boid = f.GetComponent<Boid>();
            boid.group = this;
            boids[count++] = boid;
        }
    }
}
