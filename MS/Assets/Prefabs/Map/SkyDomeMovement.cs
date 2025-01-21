using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDomeMovement : MonoBehaviour
{
    public Texture[] myTexture;
    private Material material;

    public float framerate = 1f / 30f;
    public float time = 0f;
    public int index = 0;
    public int maxIndex = 0;
    private void Start()
    {
        time = 0f;
        index = 0;
        maxIndex = myTexture.Length - 1;
        material = GetComponent<MeshRenderer>().material;
    }
    private void FixedUpdate()
    {
        if (time < framerate) { time += Time.deltaTime; }
        else 
        { 
            time -= framerate;
            if (index < maxIndex) { index++; }
            else { index -= maxIndex; }
        }


        material.SetTexture("_MainTex", myTexture[index]);
    }
}
