using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    public PlayerData playerData;
    public PlayerPrefabs playerPrefabs;

    static CharacterSettings instance;
    public static CharacterSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));

            }
            return instance;
        }
    }


}
