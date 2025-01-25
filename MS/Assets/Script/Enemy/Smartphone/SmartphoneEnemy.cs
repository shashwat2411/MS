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

        stopLooking = true;
        stopRotation = false;

        ScaleUp();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        stopRotation = false;
        RotateTowards(player.transform.position);
    }

    public override void Death()
    {
        base.Death();

        GetComponent<Collider>().enabled = false;
        animator.speed = 0f;

        StartCoroutine(hand.DissolveOut(dissolveOutDuration));
        StartCoroutine(screen.DissolveOut(dissolveOutDuration));
        StartCoroutine(phone.DissolveOut(dissolveOutDuration));
        Destroy(gameObject, dissolveOutDuration);
    }
    
    public EnemyPoison GetItem() { return (EnemyPoison)itemReference; }

}