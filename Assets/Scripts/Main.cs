using UnityStandardAssets.Cameras;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;

public class Main : MonoBehaviour
{


    public enum BeatType
    {
        Kick,
        Snare,
        Treeble
    }
    public BeatType beatType;
    
    public AudioController audioController;
    public ParticleGroup particleGroup;
    public FreeLookCam camRig;
    public bool syncShake;
    public bool syncZoom;
    public float kickMultiplier;
    public float snareMultiplier;
   
    
    private float zoomDest;
    private float zoomInit;


    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 30;
        zoomInit = camRig.zoomSpeed;

    }
	
	// Update is called once per frame
	void FixedUpdate () {


        if (Input.GetMouseButtonDown(0) || audioController.kick || audioController.snare) {
            float attractionMult = audioController.kick ? kickMultiplier : -snareMultiplier;
            particleGroup.beatReaction(attractionMult, audioController.kick,  audioController.snare);
            

            //if (syncZoom)
            //    zoomDest = 20;
        }
        else
        {
            zoomDest = zoomInit;
        }
        


        //camRig.shakeAmount += (shakeDest - camRig.shakeAmount) * 0.5f * (Time.deltaTime * 50);
        //camRig.zoomSpeed += (zoomDest - camRig.zoomSpeed) * 0.01f;
    }
}
