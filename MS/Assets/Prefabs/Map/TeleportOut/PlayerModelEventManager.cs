using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBase;

public class PlayerModelEventManager : MonoBehaviour
{
    public TeleportOutCutScene teleporter;
    public ScreenShatter shatterer;

    [Header("Material")]
    public EnemyMaterial limbs;
    public EnemyMaterial head;
    public EnemyMaterial watch;
    public EnemyMaterial body;
    public EnemyMaterial helmet;

    private void Start()
    {
        //limbs.InstantiateMaterial();
        //head.InstantiateMaterial();
        //watch.InstantiateMaterial();
        //body.InstantiateMaterial();
        //helmet.InstantiateMaterial();
    }

    public void PlayerReset()
    {
        teleporter.mainCharacter.SetBool("dissolveOut", false);
        teleporter.mainCharacter.SetBool("dissolveIn", false);
        teleporter.mainCharacter.SetBool("deathDissolveOut", false);
    }

    public void StartScreenShatter()
    {
        StartCoroutine(shatterer.ShatterScreenInitate());
    }

    public void StartDeathScreenShatter()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(shatterer.FailShatterScreenInitate());
    }
}
