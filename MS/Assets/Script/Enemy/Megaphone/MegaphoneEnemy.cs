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

    //___���z�֐���Override_________________________________________________________________________________________________________________________
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