using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPermanentAblityCollector : MonoBehaviour, IAtkEffBonusAdder
{
    public List<GameObject> all = new List<GameObject>();

    void Start()
    {
        var save = PlayerSave.Instance.playerAblities;
        if(save != null)
        {
            all = save;
            foreach (var item in all)
            {
                if(item != null)
                {
                    Instantiate(item, transform.position, transform.rotation);
                }
                
            }
        }



    }

    public void ApplyBonus(GameObject bonusEffect)
    {
        //var be = ObjectPool.Instance.Get(bonusEffect, transform.position, transform.rotation);
        var be = Instantiate(bonusEffect,transform.position, transform.rotation);
        all.Add(bonusEffect);
    }
    public void ResetBonus()
    {
        all.Clear();
    }

}
