using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SmartphoneEnemy : ThrowEnemy
{
    [Header("Material")]
    public EnemyMaterial hand;
    public EnemyMaterial screen;
    public EnemyMaterial phone;

    //___âºëzä÷êîÇÃOverride_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        hand.InstantiateMaterial();
        screen.InstantiateMaterial();
        phone.InstantiateMaterial();

        float scale = transform.localScale.x;
        hand.SetMaxDissolveScale(scale);
        screen.SetMaxDissolveScale(scale);
        phone.SetMaxDissolveScale(scale);
    }
    protected override void Start()
    {
        base.Start();

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