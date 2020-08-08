using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    
    [Range(0, 30)]
    public int kickSample;
    [Range(0, 1)]
    public float kickThreshold;

    [Range(0, 30)]
    public int snareSample;
    [Range(0, 1)]
    public float snareThreshold;

    [Range(0, 30)]
    public int treebleSample;
    [Range(0, 1)]
    public float treebleThreshold;

    public bool useMeanLevels;

    private AudioSpectrum audioSpectrum;
    private bool _kick;
    private bool _snare;
    private bool _treeble;

    public bool kick { get { return _kick;} }
    public bool snare { get { return _snare; } }
    public bool treeble { get { return _treeble; } }

    void Start()
    {
        audioSpectrum = GetComponent<AudioSpectrum>();
    }

    // Update is called once per frame
    void Update()
    {
        float[] levels = useMeanLevels ? audioSpectrum.MeanLevels : audioSpectrum.PeakLevels;
        _kick = levels[kickSample] > kickThreshold;
        _snare = levels[snareSample] > snareThreshold;
        _treeble = levels[treebleSample] > treebleThreshold;

    }

    public bool getBeat(int type)
    {
        switch (type)
        {
            case 0:
                return _kick;
            case 1:
                return _snare;
            case 2:
                return _treeble;
        }

        return false;
    }
}
