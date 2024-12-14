using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFireFactory : SpecialAttackFactory
{
    public List<float> declineIntervalData = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Initiate(int count, Vector3 pos, Quaternion rot, float lifetime = 0.8F, float damage = 1, ChargePhase chargePhase = ChargePhase.Entry)
    {
        int i = levelData[level].count;
        if (count != 0)
        {
            i = count;
        }

        for (int j = 0; j < i; j++)
        {
            var obj = ObjectPool.Instance.Get(spAtk, pos, rot);
            obj.GetComponent<GroundFire>().Initiate(1.0f, damage * levelData[level].damageFactor, declineIntervalData[level]);
            Debug.Log("sp level:  " + level + " sp damage factor " + levelData[level].damageFactor);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LevelUp()
    {
        base.LevelUp();
        
    }
}
