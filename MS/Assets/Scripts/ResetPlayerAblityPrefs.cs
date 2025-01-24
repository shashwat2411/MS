using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayerAblityPrefs : MonoBehaviour
{
 
    PlayerPrefabs playerPrefabs;

    // Start is called before the first frame update
    void Start()
    {
      

    }

    void ResetPlayer()
    {
        playerPrefabs = CharacterSettings.Instance.playerPrefabs;

        playerPrefabs.ResetPlayerPrefabs();

        PlayerSave.Instance.Clear();
    }

    private void OnDestroy()
    {
        ResetPlayer();
    }
}
