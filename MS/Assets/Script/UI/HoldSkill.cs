using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSkill : MonoBehaviour
{
    


    //public List<BonusData> PlayerBonusData;
    public List<BonusItem> PlayerBonusItem;

    PlayerManager player;


    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

   

    public void AddPlayerBonusItem(BonusItem item)
    {
        bool IsHold = false;

        foreach (var e in PlayerBonusItem)
        {
            if (item.name == e.name)
            {
                IsHold = true;
            }
        }

        if (IsHold == false)
        {
            PlayerBonusItem.Add(item);
        }
    }
}
