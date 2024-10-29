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
     private Color endColor = Color.red;
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
        //if (start)
        //{
        //    matInstance.SetColor("_EmissionColor", Color.Lerp(nullColor, endColor * intensity, Mathf.PingPong(Time.time, flashSpeed)));
        //}
        //else
        //{
        //    matInstance.SetColor("_EmissionColor", nullColor);
        //}

    }

     private void OnDisable()
     {
        Death();
         Destroy(matInstance);
     }

    public void Death()
    {
        matInstance.SetColor("_EmissionColor", Color.black);
    }

    public IEnumerator HurtFlash()
    {
        var tc = endColor * intensity;
        float x = 1.0f;
        float colorValue = tc.r / x;
        matInstance.SetColor("_EmissionColor", tc);
        yield return 0;
        while (tc.r > 0)
        {
            x += flashSpeed * Time.deltaTime;
            tc.r = tc.r > 0.02? tc.r / x : 0;
            matInstance.SetColor("_EmissionColor", tc);
            
            yield return 0;
        }

    }

 }
