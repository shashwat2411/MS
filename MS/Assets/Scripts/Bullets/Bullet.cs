using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;




public class Bullet : BulletBase
{
    public override void DoSpecialThings() 
    {

        if (spFactory.Count <= 0)
        {
            return;
        }
        // �S��
        if (chargePhase == ChargePhase.Max || chargePhase == ChargePhase.High)
        {

            foreach (var g in spFactory)
            {
                g.Initiate(0, transform.position, transform.rotation, 1.0f, this.damage / 2.0f, chargePhase, transform);
            }
        }
        //�@1����
        else if (chargePhase == ChargePhase.Middle)
        { 
            foreach(var g in spFactory)
            {
                g.Initiate(1, transform.position, transform.rotation, 1.0f, this.damage / 2.0f, chargePhase, transform);
            }
        }
        // �����_��1��
        else if(chargePhase == ChargePhase.Low)
        {
            for(int i=0;i<(int)chargePhase;i++)
            {
              var index = Random.Range(0, spFactory.Count);
               spFactory[index].Initiate(1,transform.position,transform.rotation, 1.0f, this.damage / 2.0f, chargePhase, transform);
                
            }
        }


    }
}
