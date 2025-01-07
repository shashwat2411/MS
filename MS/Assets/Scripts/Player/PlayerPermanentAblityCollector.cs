using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPermanentAblityCollector : MonoBehaviour, IAtkEffBonusAdder
{
    public List<GameObject> all = new List<GameObject>();
    public void ApplyBonus(GameObject bonusEffect)
    {
        //var be = ObjectPool.Instance.Get(bonusEffect, transform.position, transform.rotation);
        var be = Instantiate(bonusEffect,transform.position, transform.rotation);
        all.Add(be);
    }
    public void ResetBonus()
    {
        all.Clear();
    }

}
