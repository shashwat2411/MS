using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int lv;

    public float hp;

    public float maxHp;


    public float mp;

    public float maxMp;


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

    [Header("�`���[�W��")]
    public float charge;

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


    [Header("�_�b�V�����G����(�_�b�V�����Ԉȉ�!)")]
    public float dashInvincibilityTime;

    [Header("�_���[�W�H��������Ƃ̖��G����")]
    public float hurtInvincibilityTime;


    public float this[PlayerDataType key]
    {
        get
        {
            if (key == PlayerDataType.hp) return hp;
            else if (key == PlayerDataType.maxHp) return maxHp;
            else if (key == PlayerDataType.mp) return mp;
            else if (key == PlayerDataType.maxMp) return maxMp;
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
            else if (key == PlayerDataType.mp) mp = value;
            else if (key == PlayerDataType.maxMp) maxMp = value;
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

        if (bs.type==PlayerBonusType.add)
        {
            this[bs.key] += bs.value;
        }
        else if(bs.type == PlayerBonusType.multiple)
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

    GameObject playerAblities;
    public GameObject bullet;
    public GameObject attackArea;
    public GameObject mpAttackArea;
  



    // �e�A�C�e���̃{�[�i�X���X�g�̃C���f�b�N�X���L�^���邽�߂�Dictionary
    [SerializeField]
   �@Dictionary<string,int> itemCountPair = new Dictionary<string, int>();


    public GameObject this[PlayerPrafabType key]
    {
        get
        {
            if (key == PlayerPrafabType.playerPermanentAblity) return playerAblities;
            else if (key == PlayerPrafabType.bullet) return bullet;
            else if (key == PlayerPrafabType.attackArea) return attackArea;
            else if (key == PlayerPrafabType.mpAttackArea) return mpAttackArea;
            else return null;
        }
        set
        {
            if (key == PlayerPrafabType.playerPermanentAblity) playerAblities = value;
            else if (key == PlayerPrafabType.bullet) bullet = value;
            else if (key == PlayerPrafabType.mpAttackArea) mpAttackArea = value;
            
        }

    }

    public PlayerPrefabs GetCopy()
    {
        return (PlayerPrefabs)MemberwiseClone();
    }


    public void ResetPlayerPrefabs()
    {
        itemCountPair.Clear();

        foreach (PlayerPrafabType val in Enum.GetValues(typeof(PlayerPrafabType)))
        {
            if (this[val] != null)
            {
                var resetItem = this[val].GetComponent<IAtkEffBonusAdder>();
                if (resetItem != null)
                {
                    resetItem.ResetBonus();
                }
            }

        }

        foreach(var item in BonusSettings.Instance.playerBonusItems)
        {
            var iAtkEffect = item.bonusList[0].BonusOnOneTime[0].bonusItem.GetComponent<IAtkEffect>();
            if (iAtkEffect != null)
            {
                iAtkEffect.ResetLevel();
            }
        }


    }

    /// <summary>
    /// �A�C�e�������ւ���
    /// </summary>
    /// <param name="replaceData"></param>
    public void ApplyReplace(ReplaceData replaceData)
    {
        this[replaceData.key] = replaceData.replaceItem;
    }

    /// <summary>
    /// �A�C�e���̈��̃{�[�i�X��K�p����
    /// </summary>
    /// <param name="item"></param>
    public void GetTopItemBonus(BonusItem item)
    {
 
        if (!itemCountPair.ContainsKey(item.name))
        {
            itemCountPair.Add(item.name, 0);
        }

        var index = itemCountPair[item.name];
        if (item.bonusList.Count > index)
        {
            var now = item.bonusList[index];
            ApplyBonus(now.BonusOnOneTime);


            // �������X�V
            // TODO�F���ږ���̃{�[�i�X�̐�������ǂݍ��݂̂�?
            item.description =  item.bonusList[index].description;
            itemCountPair[item.name]++;
        }

    }


    /// <summary>
    /// ���ׂẴ{�[�i�X���J��Ԃ�
    /// </summary>
    /// <param name="bonusItem"></param>
    protected void ApplyBonus(List<BonusItemStats> bonusItem)
    {
        foreach (var bs in bonusItem)
        {
            ApplyBonus(bs);
        }
    }


    /// <summary>
    /// ��̓I�Ƀ{�[�i�X��K�p����
    /// </summary>
    /// <param name="bis"></param>
    private void ApplyBonus(BonusItemStats bis)
    {
        if(bis.type == ItemBonusType.levelUp)
        {
            // Upgrade requires this item first
            var atkEff = bis.bonusItem.GetComponent<IAtkEffect>();
            if (atkEff != null)
            {
                atkEff.LevelUp();
            }
        }
        else  if(bis.type == ItemBonusType.item)
        {
            this[bis.key].GetComponent<IAtkEffBonusAdder>().ApplyBonus(bis.bonusItem);
        }
       
    }

}

