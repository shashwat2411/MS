using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBullet : Bullet
{
    [SerializeField]
    GameObject lightingArea;


    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(this.damage);
        Debug.Log(this.hitPos);

    }


    public override void SpecialEffect() 
    {
        Debug.Log("test special");
    }


}
