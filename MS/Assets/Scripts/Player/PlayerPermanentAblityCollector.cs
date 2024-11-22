using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPermanentAblityCollector : MonoBehaviour, IAtkEffBonusAdder
{
    public void ApplyBonus(GameObject bonusEffect)
    {
        var be = ObjectPool.Instance.Get(bonusEffect, transform.position, transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
