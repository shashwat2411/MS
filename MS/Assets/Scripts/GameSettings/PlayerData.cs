using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int lv;

    public float hp;

    public float maxHp;

    [Header("今の経験値")]
    public float exp;

    [Header("必要な経験値")]
    public float nextExp;

    [Header("攻撃力")]
    public float attack;

    [Header("今の消費能力の回数")]
    public float specialAttackTime;

    [Header("今の消費能力の回数")]
    public float maxSpecialAttackTime;

    [Header("防御力")]
    public float defence;

    [Header("回復量")]
    public float healthRespons;

    [Header("最大チャージ時間")]
    public float maxChargeTime;

    [Header("チャージなし最初出る攻撃範囲")]
    public float maxAimSize;

    [Header("最大攻撃範囲")]
    public float maxAttackSize;

    [Header("チャージ速度")]
    public float chargeSpeed;

    [Header("攻撃範囲の移動速度")]
    public float atkMoveSpeed;

    [Header("ダッシュ時間")]
    public float dashTime;

    [Header("ダッシュCD")]
    public float dashCooldown;
    public float this[PlayerDataType key]
    {
        get
        {
            if (key == PlayerDataType.hp) return hp;
            else if (key == PlayerDataType.maxHp) return maxHp;
            else if (key == PlayerDataType.exp) return exp;
            else if (key == PlayerDataType.attack) return attack;
            else if (key == PlayerDataType.specialAttackTime) return specialAttackTime;
            else if (key == PlayerDataType.maxSpecialAttackTime) return maxSpecialAttackTime;
            else if (key == PlayerDataType.defence) return defence;
            else if (key == PlayerDataType.healthRespons) return healthRespons;
            else if (key == PlayerDataType.maxChargeTime) return maxChargeTime;
            else if (key == PlayerDataType.maxAimSize) return maxAimSize;
            else if (key == PlayerDataType.maxAttackSize) return maxAttackSize;
            else if (key == PlayerDataType.chargeSpeed) return chargeSpeed;
            else if (key == PlayerDataType.atkMoveSpeed) return atkMoveSpeed;
            else if (key == PlayerDataType.dashTime) return dashTime;
            else if (key == PlayerDataType.dashCooldown) return dashCooldown;
            else return 0;
        }


        set
        {
            if (key == PlayerDataType.hp) hp = value;
            else if (key == PlayerDataType.maxHp) maxHp = value;
            else if (key == PlayerDataType.exp) exp = value;
            else if (key == PlayerDataType.attack) attack = value;
            else if (key == PlayerDataType.specialAttackTime) specialAttackTime = value;
            else if (key == PlayerDataType.maxSpecialAttackTime) maxSpecialAttackTime = value;
            else if (key == PlayerDataType.defence) defence = value;
            else if (key == PlayerDataType.healthRespons) healthRespons = value;
            else if (key == PlayerDataType.maxChargeTime) maxChargeTime = value;
            else if (key == PlayerDataType.maxAimSize) maxAimSize = value;
            else if (key == PlayerDataType.maxAttackSize) maxAttackSize = value;
            else if (key == PlayerDataType.chargeSpeed) chargeSpeed = value;
            else if (key == PlayerDataType.atkMoveSpeed) atkMoveSpeed = value;
            else if (key == PlayerDataType.dashTime) dashTime = value;
            else if (key == PlayerDataType.dashCooldown) dashCooldown = value;
        }
    }

    /// <summary>
    /// ボーナス適用
    /// </summary>
    /// <param name="bs"></param>
    public void ApplyBonus(BonusData bd)
    {
       foreach(var bs in bd.bonuses)
       {
           ApplyBonus(bs);
       }
    }

    /// <summary>
    /// ボーナス適用
    /// </summary>
    /// <param name="bs"></param>
    protected void ApplyBonus(BonusStats bs)
    {
        // TODO: 加算以外のボーナス処理

        // TODO: 最大値チェック

        if (bs.type==BonusType.add)
        {
            this[bs.key] += bs.value;
        }
        else if(bs.type == BonusType.multiple)
        {
            this[bs.key] *= (1 + bs.value);
        }

    }


    public PlayerData GetCopy()
    {
        return (PlayerData)MemberwiseClone();
    }

}


[System.Serializable]
public class PlayerPrefabs
{
   public GameObject bullet;

    public GameObject this[PlayerPrafabType key]
    {
        get
        {
            if (key == PlayerPrafabType.bullet) return bullet;
            else return null;
        }
        set
        {
            if (key == PlayerPrafabType.bullet) bullet = value;
            
        }

    }


    public PlayerPrefabs GetCopy()
    {
        return (PlayerPrefabs)MemberwiseClone();
    }


    public void ApplyReplace(ReplaceData replaceData)
    {
        this[replaceData.key] = replaceData.replaceItem;
    }
}

