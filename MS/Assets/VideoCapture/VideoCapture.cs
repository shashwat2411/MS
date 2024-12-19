using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCapture : MonoBehaviour
{
    public Animator scene;
    public Animator model;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            scene.StopPlayback();
            model.StopPlayback();

            scene.Play("CutScene");
            model.Play("Take 001");
        }
    }
}
