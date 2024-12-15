using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LightingFactory : SpecialAttackFactory
{
    public override void Initiate(int count, Vector3 pos, Quaternion rot, float lifetime = 0.8F, float damage = 1, ChargePhase chargePhase = ChargePhase.Entry, Transform usedMenko = null)
    {
        int i = 3;
        if (level == 0) { i = 1; }
        else if (level == 1 || level == 2) { i = 2; }
        else { i = 3; }

        //int i = levelData[level].count;
        //if (count != 0)
        //{
        //    i = count;
        //}

        for (int j = 0; j < i; j++)
        {
            Lighting obj = ObjectPool.Instance.Get(spAtk, pos, rot).GetComponent<Lighting>();

            obj.SetOrder(j);
            obj.Initiate(1.0f, damage * levelData[level].damageFactor, level, levelData.Count, usedMenko);

            Debug.Log("sp level:  " + level + " sp damage factor " + levelData[level].damageFactor);
        }
    }
}
