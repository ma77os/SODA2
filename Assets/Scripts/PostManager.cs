using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostManager : MonoBehaviour {

    public float bloomIntensity = 9f;

    private Bloom bloomLayer;
    private PostProcessVolume volume;

	// Use this for initialization
	void Start () {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
	}
	
	// Update is called once per frame
	void Update () {
        bloomLayer.intensity.value = bloomIntensity;

    }
}
