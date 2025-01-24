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
        playerPrefabs = CharacterSettings.Instance.playerPrefabs;

        playerPrefabs.ResetPlayerPrefabs();

        PlayerSave.Instance.Clear();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }
}
