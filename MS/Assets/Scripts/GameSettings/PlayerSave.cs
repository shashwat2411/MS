using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSave", menuName = "ScriptableObjects/PlayerSave")]
public class PlayerSave : ScriptableObject
{
    public PlayerData playerData;
    public List<GameObject> playerAblities = new List<GameObject>();

    public (int, int) minimapPos;

    static PlayerSave instance;
    public static PlayerSave Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<PlayerSave>(nameof(PlayerSave));

            }
            return instance;
        }
    }


    public void Clear()
    {
        playerAblities.Clear();   
        playerData = CharacterSettings.Instance.playerData;
        minimapPos = (0, 0);

    }

}
