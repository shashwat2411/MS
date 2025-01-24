using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MegaphoneEnemy : ThrowEnemy
{
    [Header("Material")]
    public EnemyMaterial megaphone;
    public EnemyMaterial body;

    //___âºëzä÷êîÇÃOverride_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        megaphone.InstantiateMaterial();
        body.InstantiateMaterial();

        float scale = transform.localScale.x;
        megaphone.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
    }

    protected override void Start()
    {
        base.Start();

        stopLooking = true;
        stopRotation = false;

        ScaleUp();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        RotateTowards(player.transform.position);
    }

    protected override void Idle()
    {
        stopRotation = true;
        stopMovement = false;

        CheckState();
    }
    protected override void Move()
    {
        stopRotation = false;
        base.Move();
    }

    protected override void Attack()
    {
        stopRotation = true;
    }

    public override void Death()
    {
        base.Death();

    }
}