using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFireFactory : SpecialAttackFactory
{
    public List<float> declineIntervalData = new List<float>();

    public override void Initiate(int count, Vector3 pos, Quaternion rot, float lifetime = 0.8F, float damage = 1, ChargePhase chargePhase = ChargePhase.Entry, Transform usedMenko = null)
    {
        int i = levelData[level].count;
        if (count != 0)
        {
            i = count;
        }

        for (int j = 0; j < i; j++)
        {
            var obj = ObjectPool.Instance.Get(spAtk, pos, rot);
            obj.GetComponent<GroundFire>().Initiate(1.0f, damage * levelData[level].damageFactor, declineIntervalData[level],usedMenko);
            Debug.Log("sp level:  " + level + " sp damage factor " + levelData[level].damageFactor);
        }
    }
}
