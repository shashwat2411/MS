using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int lv;

    public float hp;

    public float maxHp;

    [Header("���̌o���l")]
    public float exp;

    [Header("�K�v�Ȍo���l")]
    public float nextExp;

    [Header("�U����")]
    public float attack;

    [Header("���̏���\�͂̉�")]
    public float specialAttackTime;

    [Header("���̏���\�͂̉�")]
    public float maxSpecialAttackTime;

    [Header("�h���")]
    public float defence;

    [Header("�񕜗�")]
    public float healthRespons;

    [Header("�ő�`���[�W����")]
    public float maxChargeTime;

    [Header("�`���[�W�Ȃ��ŏ��o��U���͈�")]
    public float maxAimSize;

    [Header("�ő�U���͈�")]
    public float maxAttackSize;

    [Header("�`���[�W���x")]
    public float chargeSpeed;

    [Header("�U���͈͂̈ړ����x")]
    public float atkMoveSpeed;

    [Header("�_�b�V������")]
    public float dashTime;

    [Header("�_�b�V��CD")]
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
    /// �{�[�i�X�K�p
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
    /// �{�[�i�X�K�p
    /// </summary>
    /// <param name="bs"></param>
    protected void ApplyBonus(BonusStats bs)
    {
        // TODO: ���Z�ȊO�̃{�[�i�X����

        // TODO: �ő�l�`�F�b�N

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

