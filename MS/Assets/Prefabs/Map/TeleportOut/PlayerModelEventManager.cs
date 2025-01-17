using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBase;

public class PlayerModelEventManager : MonoBehaviour
{
    public TeleportOutCutScene teleporter;

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
        teleporter.player.SetBool("dissolveOut", false);
        teleporter.player.SetBool("dissolveIn", false);
    }
}
