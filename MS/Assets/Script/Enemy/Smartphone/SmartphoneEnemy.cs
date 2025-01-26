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

        ResetEnemy();
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
    public void ResetEnemy()
    {
        hand.renderer.enabled = false;
        screen.renderer.enabled = false;
        phone.renderer.enabled = false;

        hand.SetDissolveToMin();
        screen.SetDissolveToMin();
        phone.SetDissolveToMin();
    }
    public override IEnumerator DissolveIn(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);

        yield return null;

        hand.renderer.enabled = true;
        screen.renderer.enabled = true;
        phone.renderer.enabled = true;

        StartCoroutine(hand.DissolveIn(duration));
        StartCoroutine(screen.DissolveIn(duration));
        StartCoroutine(phone.DissolveIn(duration));

        yield return new WaitForSeconds(duration);

        GetComponent<BoxCollider>().enabled = true;
    }
    public EnemyPoison GetItem() { return (EnemyPoison)itemReference; }

}