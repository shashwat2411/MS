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

        float scale = transform.localScale.x;
        megaphone.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
    }

    protected override void Start()
    {
        base.Start();

        megaphone.InstantiateMaterial();
        body.InstantiateMaterial();

        ScaleUp();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Death()
    {
        base.Death();

    }
}