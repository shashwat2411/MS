using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

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

        ears.InstantiateMaterial();
        hands.InstantiateMaterial();
        body.InstantiateMaterial();
        tv.InstantiateMaterial();
        screen.InstantiateMaterial();

        float scale = transform.localScale.x;
        ears.SetMaxDissolveScale(scale);
        hands.SetMaxDissolveScale(scale);
        body.SetMaxDissolveScale(scale);
        tv.SetMaxDissolveScale(scale);
        screen.SetMaxDissolveScale(scale);

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

        StartCoroutine(ears.DissolveOut(dissolveOutDuration));
        StartCoroutine(hands.DissolveOut(dissolveOutDuration));
        StartCoroutine(body.DissolveOut(dissolveOutDuration));
        StartCoroutine(tv.DissolveOut(dissolveOutDuration));
        StartCoroutine(screen.DissolveOut(dissolveOutDuration));

        Destroy(gameObject, dissolveOutDuration);
    }
    public void ResetEnemy()
    {
        ears.renderer.enabled = false;
        hands.renderer.enabled = false;
        body.renderer.enabled = false;
        tv.renderer.enabled = false;
        screen.renderer.enabled = false;

        ears.SetDissolveToMin();
        hands.SetDissolveToMin();
        body.SetDissolveToMin();
        tv.SetDissolveToMin();
        screen.SetDissolveToMin();
    }
    public override IEnumerator DissolveIn(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);

        yield return null;

        ears.renderer.enabled = true;
        hands.renderer.enabled = true;
        body.renderer.enabled = true;
        tv.renderer.enabled = true;
        screen.renderer.enabled = true;

        StartCoroutine(ears.DissolveIn(duration));
        StartCoroutine(hands.DissolveIn(duration));
        StartCoroutine(body.DissolveIn(duration));
        StartCoroutine(tv.DissolveIn(duration));
        StartCoroutine(screen.DissolveIn(duration));

        yield return new WaitForSeconds(duration);

        GetComponent<BoxCollider>().enabled = true;
    }
    public EnemyBomb GetItem() { return (EnemyBomb)itemReference; }
}