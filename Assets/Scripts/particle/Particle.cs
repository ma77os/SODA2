using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Mesh[] meshes;

    private float life = 0;
    private float lifetime;
    private float startTime;
    private float speed;
    private Vector3 currentForce;
    private Transform currentShape;
    private int initIndexShape;
    private int currentIndexShape = -1;
    private Color color;
    private float metallic;
    private float smoothness;
    private ParticleGroup group;
    private int totalShapes;
    private bool attractable;
    private MeshFilter meshFilter;
    private Renderer rend;
    private Rigidbody rb;
    private bool _active;
    private BoxCollider collider;

    public bool active{ get { return _active; }    }

    void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
        collider = GetComponent<BoxCollider>();
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        totalShapes = meshes.Length;


        changeShape(initIndexShape);
        birth();
        _active = true;
    }

    public void init(ParticleGroup group, int indexShape, Color color, float metallic, float smoothness)
    {
        this.group = group;
        this.initIndexShape = indexShape;
        this.color = color;
        this.metallic = metallic;
        this.smoothness = smoothness;

    }

    void birth()
    {
        speed = Random.Range(group.speedMin, group.speedMax) / 1000;
        currentForce = group.directionFlow * speed;
        lifetime = Random.Range(group.lifetimeMin, group.lifetimeMax);


        attractable = Random.value <= group.percentageAttraction;


        acceleration = new Vector3();
        velocity = new Vector3();

        //float angle = Random.value * MathExt.TWO_PI;
        //bool isRadiusRandom = Random.value < 0.2f;
        //float radiusStart = isRadiusRandom ? 0 : 0.7f;
        //float radiusAmount = isRadiusRandom ? 1 : 0.3f;
        //float radius = (radiusStart + Random.value * radiusAmount) * group.birthArea.x;
        //location = group.transform.position + new Vector3(Mathf.Cos(angle) * radius, Random.Range(-group.birthArea.y, group.birthArea.y), Mathf.Sin(angle) * radius);
        location = transform.position;
        rb.MovePosition(location);

        startTime = Time.time;
    }

    void FixedUpdate()
    {
        //acceleration = currentForce;

        velocity += currentForce;
        velocity = Vector3.Scale(velocity, group.friction);
        location += velocity;
        acceleration *= 0;

        //transform.position = location;
        //rigidbody.MovePosition(location);
        rb.velocity = velocity * 50;

        life = MathExt.Map(Time.time - startTime, 0, lifetime, 0, 1);

        if (life > 1)
        {
            //birth();
            Destroy(gameObject);
        }
    }

    public void attract(Vector3 target, float force, float bound)
    {
        if (!attractable) return;

        Vector3 targetVec = target - location;
        float dist = targetVec.magnitude;
        float distBound = dist - bound;
        targetVec.Normalize();
        Vector3 attractVec = new Vector3(targetVec.x * distBound, targetVec.y * distBound, targetVec.z * distBound);
        attractVec *= force;
        velocity += attractVec;

    }

    public void randomShape()
    {
        changeShape(Random.Range(0, totalShapes-1));

    }

    public void changeShape(int index)
    {
        if (index != currentIndexShape)
        {
            currentIndexShape = index < 0 ? 0 : index > totalShapes -1 ? totalShapes -1 : index;
            meshFilter.mesh = meshes[currentIndexShape];
            //if(currentIndexShape == 1)
            //{
            //    collider.size = new Vector3(0.2f, 4, 0.2f);
            //}
            //else
            //{
            //    collider.size = new Vector3(1, 1, 1);

            //}
           // collider.size = meshFilter.mesh.bounds.size;
        }

        treatShape();

        //if (index != currentIndexShape)
        //{
        //    if (currentShape)
        //        currentShape.gameObject.SetActive(false);

        //    currentIndexShape = index;
        //    currentShape = transform.GetChild(currentIndexShape);
        //    currentShape.gameObject.SetActive(true);
        //}

        // if (currentShape)
        //   treatShape();
    }

    public void changeColor(Color color, float metallic, float smoothness)
    {
        this.color = color;
        this.metallic = metallic;
        this.smoothness = smoothness;
    }

    void treatShape()
    {
        rend.material.SetColor("_Color", color);
        rend.material.SetColor("_EmissionColor", Random.value < 0.5 ? color : Color.black);
        rend.material.SetFloat("_Metallic", this.metallic);
        //rend.material.SetFloat("_Glossiness", Random.Range(0.7f, 1));

        Quaternion r = Quaternion.Euler(new Vector3(Random.value * 360, Random.value * 360, Random.value * 360));

        switch (currentIndexShape)
        {
            case 0:
                if (group.lineRotation != -1)
                {
                    r = Quaternion.Euler(new Vector3(90, 0, 0));
                }
                break;

            case 1:
            case 3:

                if (group.lineRotation != -1)
                {
                    r = Quaternion.Euler(new Vector3(0, 0, group.lineRotation));
                }

                break;
        }

        rb.MoveRotation(r);
        //transform.localRotation = r;


    }
}
