using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallete : MonoBehaviour {

    public Color[] colors;
    private int c = 0;

    public Color random()
    {
        return colors[Random.Range(0, colors.Length-1)];
    }

    public Color next()
    {
        if (c++ > colors.Length - 1) c = 0;
        return current;
    }

    public Color current
    {
        get
        {
            return colors[c];
        }
    }

    public void shuffle()
    {
        // TODO
    }


}
