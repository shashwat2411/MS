using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


 public class HighLightFlash : MonoBehaviour
 {
    public bool start=false;
    public float flashSpeed;
     public float intensity;
    Color nullColor;
     public Color startColor;
     public Color endColor;
     public Material mat;
     private Material matInstance;

 
     private void OnEnable()
     {
         
         matInstance = new Material(mat);
         matInstance.EnableKeyword("_EMISSION");
     
         GetComponent<Renderer>().material = matInstance;
     }

     private void Update()
     {
        if(start)
            matInstance.SetColor("_EmissionColor", Color.Lerp(nullColor, endColor * intensity, Mathf.PingPong(Time.time, flashSpeed)));
        else
            matInstance.SetColor("_EmissionColor", nullColor);


    }

     private void OnDisable()
     {
         Destroy(matInstance);
     }
 }
