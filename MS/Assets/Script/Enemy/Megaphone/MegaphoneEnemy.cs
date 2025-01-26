using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

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

        if (player.GetComponent<PlayerManager>().tutorial == true)
        {
            ResetEnemyTutorial();
        }
        else
        {
            ResetEnemy();
        }

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

    }

    public override void Death()
    {
        base.Death();

        GetComponent<Collider>().enabled = false;
        animator.speed = 0f;

        StartCoroutine(megaphone.DissolveOut(dissolveOutDuration));
        StartCoroutine(body.DissolveOut(dissolveOutDuration));

        Destroy(gameObject, dissolveOutDuration);
    }

    public void ResetEnemy()
    {
        megaphone.renderer.enabled = false;
        body.renderer.enabled = false;

        megaphone.SetDissolveToMin();
        body.SetDissolveToMin();
    }
    public void ResetEnemyTutorial()
    {
        GetComponent<BoxCollider>().enabled = false;

        megaphone.renderer.enabled = false;
        body.renderer.enabled = false;

        megaphone.SetDissolveToMin();
        body.SetDissolveToMin();

        this.enabled = false;
    }
    public override IEnumerator DissolveIn(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);

        yield return null;

        megaphone.renderer.enabled = true;
        body.renderer.enabled = true;
        StartCoroutine(megaphone.DissolveIn(duration));
        StartCoroutine(body.DissolveIn(duration));

        yield return new WaitForSeconds(duration);

        GetComponent<BoxCollider>().enabled = true;
        this.enabled = true;
    }

    public void SetStopRotation(bool value) { stopRotation = value; }
}