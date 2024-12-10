using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;




public class Bullet : BulletBase
{
    public override void DoSpecialThings() 
    {

        if(chargePhase == ChargePhase.Max)
        {

            foreach (var g in sp)
            {
                var obj = ObjectPool.Instance.Get(g, transform.position, transform.rotation);
                obj.GetComponent<IAtkEffect>().Initiate(1.0f, this.damage / 2.0f);
            }
        }
        else
        {
            for(int i=0;i<(int)chargePhase;i++)
            {
                if (i >= sp.Count)
                {
                    break;
                }
                else
                {
                    var obj = ObjectPool.Instance.Get(sp[i], transform.position, transform.rotation);
                    obj.GetComponent<IAtkEffect>().Initiate(1.0f, this.damage / 2.0f);
                }
            }
        }


    }
}
