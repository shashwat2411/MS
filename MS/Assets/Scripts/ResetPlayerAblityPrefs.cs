using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerAblityPrefs : MonoBehaviour
{
 
    PlayerPrefabs playerPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        playerPrefabs = CharacterSettings.Instance.playerPrefabs;

        playerPrefabs.ResetPlayerPrefabs();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
