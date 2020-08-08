using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGroup : MonoBehaviour {

    public GameObject palletesGameObj;
    public Camera camera;

    public int maxBoids;
    public GameObject boidPrefab;
    public float bornRate;
    public float lifetimeMin;
    public float lifetimeMax;
    public float speedMin;
    public float speedMax;

    public enum Shape
    {
        Cross,
        Line,
        Piramid,
        Cube,
        Sphere,
        Icosphere,
        Random
    }
    public Shape currentShape;
    
    public bool changeColors = true;
    [Range(0, 1)]
    public float percentageAttraction;
    public float attractForce;
    public Vector3 friction;
    public Vector2 birthArea;
    public Vector3 directionFlow;
    

    private Particle[] particles;
    private int totalShapes = 6;
    private int currentShapeIndex;
    private int currentPalleteIndex;
    private int count = 0;
    private Pallete[] palletes;
    private Pallete pallete;
    private float metallic;
    private float smoothness;
    private float alpha;
    private float _lineRotation;
    public float lineRotation { get { return _lineRotation; } }

    private void Awake()
    {
        Cursor.visible = false;
    }

    // Use this for initialization
    void Start ()
    {
        particles = new Particle[5000];
        currentShapeIndex = currentShape == Shape.Random ? randomShape() : (int)currentShape;

        palletes = palletesGameObj.GetComponents<Pallete>();
        currentPalleteIndex = 0;
        swapPallete(currentPalleteIndex);



    }
    void FixedUpdate() {

        createBoid(bornRate);

        
    }
    public void beatReaction(float mult, bool cShape, bool cColors)
    {
        if (cShape)
        {
            currentShapeIndex = currentShape == Shape.Random ? randomShape() : (int)currentShape;
            
        }
        if (changeColors && cColors)
        {
            randomPallete();
        }

        attract(mult, true);
    }

    void createBoid(float rate)
    {
        int amount = Mathf.FloorToInt(rate);
        for (int i = 0; i < amount; i++)
        {
            //if (count > maxBoids - 1) return;


            float angle = Random.value * MathExt.TWO_PI;
            bool isRadiusRandom = Random.value < 0.2f;
            float radiusStart = isRadiusRandom ? 0 : 0.7f;
            float radiusAmount = isRadiusRandom ? 1 : 0.3f;
            float radius = (radiusStart + Random.value * radiusAmount) * birthArea.x;
            Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle) * radius, Random.Range(-birthArea.y, birthArea.y), Mathf.Sin(angle) * radius);


            GameObject f = Instantiate(boidPrefab, pos, Quaternion.identity, transform) as GameObject;
            Particle p = f.GetComponent<Particle>();
            p.init(this, currentShapeIndex, pallete.random(), metallic, smoothness);
            //particles[count] = p;
            count++;
        }
    }

    public int randomShape()
    {
        float cousin45deg = Mathf.Floor(Random.value * 8) * 45;
        _lineRotation = Random.value * 2 < 1 ? -1 : cousin45deg;

        return Mathf.RoundToInt(Random.value * (totalShapes-1));
    }

    public void swapShape(int index)
    {
        currentShapeIndex = index;
        for (int i = 0; i < transform.childCount; i++)
        {
            Particle p = transform.GetChild(i).GetComponent<Particle>();
            if (p && p.active)
                p.changeShape(currentShapeIndex);
        }
        //foreach (Particle p in particles)
        //{
        //    if (!p) continue;
        //    p.changeShape(currentShapeIndex);
        //}
    }

    public void randomPallete()
    {
        int index = Random.Range(0, palletes.Length - 1);
        while (index == currentPalleteIndex)
        {
            index = Random.Range(0, palletes.Length - 1);
        }
        swapPallete(index);
    }

    public void swapPallete(int index)
    {
        if(index > palletes.Length - 1)
        {
            index = palletes.Length - 1;
        }
        currentPalleteIndex = index;
        pallete = palletes[index];
        
        if(index == 0)
        {
            camera.backgroundColor = Color.white;
        }
        else { 

            Color whiteOrBlack = Random.value < 0.3 ? Color.white : Color.black;

            camera.backgroundColor = Random.value < 0.5 ? pallete.random() : whiteOrBlack;
        }
    }
    

    public void attract(float mult, bool attract)
    {
        float bound = 20;
        Vector3 target = new Vector3(0, 0, 0);
        target = new Vector3(target.x + Random.Range(-bound, bound), target.y + Random.Range(-bound, bound), target.z + Random.Range(-bound, bound));
        
        float force = attractForce * mult;

        metallic = Random.value;
        smoothness = Random.value * 0.8f;
        //alpha = Random.value < 0.5 ? 0.1f : 1;

        
        //foreach (Particle p in particles)
        for(int i = 0; i < transform.childCount; i++)
        {
            Particle p = transform.GetChild(i).GetComponent<Particle>();
            if (p && p.active)
            {
                Color c = pallete.random();
                //c.a = alpha;
                p.changeColor(c, metallic, smoothness);
                p.changeShape(currentShapeIndex);
                if(attract)
                    p.attract(target, force, bound);
            }
        }
    }
}
