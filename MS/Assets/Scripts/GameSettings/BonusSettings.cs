using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusSettings", menuName = "ScriptableObjects/BonusSettings")]
public class BonusSettings : ScriptableObject
{

    public List<BonusData> bonusDatas;

    public List<ReplaceData> replaceDatas;
  
    static BonusSettings instance;
    public static BonusSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<BonusSettings>(nameof(BonusSettings));
            }
            return instance;
        }
    }
}
