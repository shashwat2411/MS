using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerDataType
{
    hp,
    maxHp,
    exp,
    attack,
    defence,
    healthRespons,
    maxChargeTime,
    maxAttackSize,
    chargeSpeed,
    atkMoveSpeed, 
    dashTime,
    dashCooldown,
}

[System.Serializable]
public class BonusData
{
    public string name;
 
    [TextArea] public string description;

    public Sprite icon;

    public List<BonusStats> bonuses;


    public BonusData GetCopy()
    {
        return (BonusData)MemberwiseClone();
    }
}


public enum BonusType
{
    add,
    multiple,
}


[System.Serializable]
public class BonusStats
{
    public BonusType type;

    public PlayerDataType key;

   
    public float value;
}
