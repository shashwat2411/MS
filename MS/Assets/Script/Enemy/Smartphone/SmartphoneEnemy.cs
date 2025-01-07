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
    protected override void Start()
    {
        base.Start();

        hand.InstantiateMaterial();
        screen.InstantiateMaterial();
        phone.InstantiateMaterial();
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