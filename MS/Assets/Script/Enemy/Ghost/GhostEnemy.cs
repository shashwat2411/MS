using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GhostEnemy : ThrowEnemy
{
    [Header("Material")]
    public EnemyMaterial ears;
    public EnemyMaterial hands;
    public EnemyMaterial body;
    public EnemyMaterial tv;
    public EnemyMaterial screen;

    //___âºëzä÷êîÇÃOverride_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        float scale = transform.localScale.x;
        ears.SetMaxDissolveScale(scale);
        hands.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
        tv.SetMaxDissolveScale(scale);
        screen.SetMaxDissolveScale(scale);
    }
    protected override void Start()
    {
        base.Start();

        ears.InstantiateMaterial();
        hands.InstantiateMaterial();
        body.InstantiateMaterial();
        tv.InstantiateMaterial();
        screen.InstantiateMaterial();

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

    public EnemyBomb GetItem() { return (EnemyBomb)itemReference; }
}