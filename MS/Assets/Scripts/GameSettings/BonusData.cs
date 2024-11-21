using JetBrains.Annotations;
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


public enum PlayerBonusType
{
    add,
    multiple,
  
}

public enum ItemBonusType
{
    levelUp,
    item,

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




[System.Serializable]
public class BonusStats
{
    public PlayerBonusType type;

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

[System.SerializableAttribute]
public class ItemBonusList
{
    public List<BonusItemStats> BonusOnOneTime = new List<BonusItemStats>();

    [TextArea] public string description;

    public ItemBonusList(List<BonusItemStats> list)
    {
        BonusOnOneTime = list;
    }
}



[System.SerializableAttribute]
public class BonusItem
{
    public string name;

    [TextArea] public string description;

    public Sprite icon;


    public List<ItemBonusList> bonusList;


    public BonusData GetCopy()
    {
        return (BonusData)MemberwiseClone();
    }
}



[System.Serializable]
public class BonusItemStats
{
    public PlayerPrafabType key;

    public ItemBonusType type;

    public GameObject bonusItem;
}
