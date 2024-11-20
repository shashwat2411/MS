using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LightingBullet : Bullet
{
    [SerializeField]
    GameObject lightingArea;



  
    public override void DoSpecialThings() 
    {
        InitiateLighting();
    }


    void InitiateLighting()
    {
        var offset =new Vector3 (Random.Range(3.0f, -1.0f), 0, Random.Range(3.0f, -1.0f));
        var obj = ObjectPool.Instance.Get(lightingArea, transform.position + offset , transform.rotation);

        Destroy(obj,1.0f);
    }


}
