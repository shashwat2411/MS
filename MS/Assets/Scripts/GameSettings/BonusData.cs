using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerDataType
{
    hp,
    maxHp,
    exp,
    attack,
    specialAttackTime,
    maxSpecialAttackTime,
    defence,
    healthRespons,
    maxChargeTime,
    maxAimSize,
    maxAttackSize,
    chargeSpeed,
    atkMoveSpeed, 
    dashTime,
    dashCooldown,
}


public enum PlayerPrafabType
{
    bullet,

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
    replace,
}


[System.Serializable]
public class BonusStats
{
    public BonusType type;

    public PlayerDataType key;

    public float value;
}


[System.Serializable]
public class ReplaceData
{
    public string name;

    [TextArea] public string description;

    public Sprite icon;

    public PlayerPrafabType key;

    public GameObject replaceItem;

}