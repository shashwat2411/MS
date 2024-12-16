using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AttackData
{
    public int count;    
    public float damageFactor; 

    public AttackData(int count, float damage)
    {
        this.count = count;
        this.damageFactor = damage;
    }
}

public class SpecialAttackFactory : MonoBehaviour
{

    public List<AttackData> levelData;
   
    protected static int level = 0;

    //List<GameObject> spAtks = new List<GameObject>();
    public GameObject spAtk;


    public virtual void Initiate(int count,Vector3 pos,Quaternion rot,float lifetime = 0.8f, float damage = 1.0f,ChargePhase chargePhase = ChargePhase.Entry, Transform usedMenko = null)
    {

    }

    public virtual void LevelUp()
    {
        if (level < levelData.Count-1)
        {
            level++;
        }

    }

    public virtual void ResetLevel()
    {
        spAtk.GetComponent<IAtkEffect>().ResetLevel();
        level = 0;
    }

}
