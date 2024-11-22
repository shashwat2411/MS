using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;




public class Bullet : BulletBase
{
    public override void DoSpecialThings() 
    {

        foreach (var g in sp)
        {
            var obj = ObjectPool.Instance.Get(g, transform.position, transform.rotation);
            obj.GetComponent<IAtkEffect>().Initiate(0.8f, this.damage / 2.0f);


        }
    }
}
