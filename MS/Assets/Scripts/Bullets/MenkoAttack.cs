using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MenkoAttack : MonoBehaviour, IAtkEffBonusAdder
{
    GameObject bullet;

    static float bulletsCount = 1;

    public float twoMenkoOffset = 1.2f;
    public float threeMenkoOffset = 2.2f;

    public void ApplyBonus(GameObject bonusEffect)
    {
        bulletsCount++;
      
    }
    public void ResetBonus()
    {
        bulletsCount = 1;
    }

    public void IniteMultiMenko(Vector3 startPoint,Transform area, float maxAttackSize,float attack,float holdtime,ChargePhase chargePhase = ChargePhase.Entry)
    {
        Debug.Log(chargePhase);
        switch (bulletsCount)
        {
            case 0:
                break;
            case 1:
                One(startPoint,area,maxAttackSize,attack,holdtime, chargePhase);
                break;
            case 2:
                Two(startPoint, area, maxAttackSize, attack, holdtime, chargePhase);
                break;
            default:
                Three(startPoint, area, maxAttackSize, attack, holdtime, chargePhase);
                break;
        }
          
    }


    void One(Vector3 startPoint, Transform area, float maxAttackSize, float attack, float holdtime, ChargePhase chargePhase)
    {
        Vector3 endPoint = new Vector3(area.position.x, 0.0f, area.position.z);
        float offset = 1.5f;


        if (holdtime < 3.5f)
        {
            endPoint = GetOffset(area.position, holdtime, offset);
        }

        var dir = endPoint - startPoint;
        dir.Normalize();

        var obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
       // var multiEndPos = GetOffset(endPoint, holdtime,8.0f);
   

       obj.GetComponent<BulletBase>().Initiate(dir, endPoint, maxAttackSize, attack * holdtime, chargePhase);

        
    }


    void Two(Vector3 startPoint, Transform area, float maxAttackSize, float attack, float holdtime, ChargePhase chargePhase)
    {
        Vector3 endPoint = new Vector3(area.position.x, 0.0f, area.position.z);
        float offset = 1.5f;


        if (holdtime < 3.5f)
        {
            endPoint = GetOffset(area.position, holdtime, offset);
        }
        var leftEnd  = endPoint - area.right * twoMenkoOffset;
        var rightEnd = endPoint + area.right * twoMenkoOffset;
        
        var leftDir = leftEnd - startPoint;
        leftDir.Normalize();
        
        var rightDir = rightEnd - startPoint;
        rightDir.Normalize();



       var obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
       obj.GetComponent<BulletBase>().Initiate(leftDir, leftEnd, maxAttackSize, attack * holdtime, chargePhase );

       obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
       obj.GetComponent<BulletBase>().Initiate(rightDir, rightEnd, maxAttackSize, attack * holdtime, chargePhase);
    }
   
    void Three(Vector3 startPoint, Transform area, float maxAttackSize, float attack, float holdtime, ChargePhase chargePhase)
    {
        Vector3 endPoint = new Vector3(area.position.x, 0.0f, area.position.z);
        float offset = 1.5f;


        if (holdtime < 3.5f)
        {
            endPoint = GetOffset(area.position, holdtime, offset);
        }
        var leftEnd =  endPoint - area.right * threeMenkoOffset;
        var rightEnd = endPoint + area.right * threeMenkoOffset;

        var leftDir = leftEnd - startPoint;
        leftDir.Normalize();

        var rightDir = rightEnd - startPoint;
        rightDir.Normalize();

        var dir = endPoint - startPoint;
        dir.Normalize();

        var obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
        obj.GetComponent<BulletBase>().Initiate(dir, endPoint, maxAttackSize, attack * holdtime, chargePhase);

        obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
        obj.GetComponent<BulletBase>().Initiate(leftDir, leftEnd, maxAttackSize, attack * holdtime, chargePhase);

        obj = ObjectPool.Instance.Get(bullet, startPoint, area.rotation);
        obj.GetComponent<BulletBase>().Initiate(rightDir, rightEnd, maxAttackSize, attack * holdtime, chargePhase);
    }




    Vector3 GetOffset(Vector3 initPos, float holdtime,float offset)
    {
        Vector3 endPoint = initPos +
              new Vector3(
                           Random.Range(-offset / holdtime, offset / holdtime),
                           0.0f,
                           Random.Range(-offset / holdtime, offset / holdtime)
                       );

        return endPoint;
    }






    public void Initiate(GameObject bullet)
    {
        this.bullet = bullet;   
        return;
    }




}
