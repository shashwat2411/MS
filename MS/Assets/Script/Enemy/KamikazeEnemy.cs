using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.XR;
using static GunEnemy;
using static ThrowEnemy;

public class KamikazeEnemy : EnemyBase
{
    public enum KAMIKAZEENEMY_STATE
    {
        IDLE,
        MOVE
    }

    [Header("State Time")]
    public KAMIKAZEENEMY_STATE state;
    public float idleTime;
    public float hurtTime;

    private AudioSource tick;

    public float lifetime = 5f;
    private float lifeCounter = 0f;
    private float soundCounter = 0f;
    public float sinMultiplier = 2f;
    public float tickDuration = 0.2f;

    public GameObject explosion;

    [ColorUsage(false, true)] public UnityEngine.Color tickColor;
    [ColorUsage(false, true)] private UnityEngine.Color originalColor;

    [Header("Material")]
    public EnemyMaterial body;

    private int _Color = Shader.PropertyToID("_Color");

    //___仮想関数のOverride_________________________________________________________________________________________________________________________
    protected override void ScaleUp()
    {
        base.ScaleUp();

        body.InstantiateMaterial();

        float scale = transform.localScale.x;
        body.SetMaxDissolveScale(scale);

        originalColor = body.material.GetColor(_Color);
    }
    protected override void Start()
    {
        base.Start();

        ScaleUp();

        tick = GetComponent<AudioSource>();

        lifeCounter = 0f;
        soundCounter = 0f;

        state = KAMIKAZEENEMY_STATE.IDLE;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        switch (state)
        {
            case KAMIKAZEENEMY_STATE.IDLE:
                Idle();
                break;

            case KAMIKAZEENEMY_STATE.MOVE:
                Follow();
                break;
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            //プレーヤーへのダメージ
            player.GetComponent<PlayerManager>().playerHP.Damage(attackPower);
            Death();
        }
    }

    public override void Damage(float value, bool killingBlow = false)
    {
        base.Damage(value, killingBlow);
    }

    public override void Death()
    {
        base.Death();
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.transform.localScale = 0.4f * Vector2.one;

        Destroy(gameObject);
    }

    private IEnumerator ChangeColor(float duration)
    {
        body.material.SetColor(_Color, tickColor);

        float elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            body.material.SetColor(_Color, UnityEngine.Color.Lerp(tickColor, originalColor, elapsed / duration));
            yield return null;
        }

        body.material.SetColor(_Color, originalColor);
    }
    //____________________________________________________________________________________________________________________________


    //____ステート________________________________________________________________________________________________________________________
    void Idle()
    {
        stopRotation = false;   //回転再会
        stopMovement = false;

        StartCoroutine(ChangeState(KAMIKAZEENEMY_STATE.MOVE, idleTime));
    }

    void Follow()
    {
        Move();

        if (lifeCounter < lifetime) { lifeCounter += Time.deltaTime; }
        else
        {
            lifeCounter = lifetime;
            Death();
        }

        soundCounter += Time.deltaTime * (lifeCounter + sinMultiplier);
        float sin = Mathf.Sin(soundCounter);
        if (Mathf.Abs(sin) >= 0.9f && tick.isPlaying == false) 
        {
            soundCounter = 0f;

            tick.Play();
            StartCoroutine(ChangeColor(tickDuration));
        }
    }
    //____________________________________________________________________________________________________________________________



    //___Coroutines_________________________________________________________________________________________________________________________
    IEnumerator ChangeState(KAMIKAZEENEMY_STATE value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        state = value;

        if (value == KAMIKAZEENEMY_STATE.IDLE)
        {
            agent.gameObject.transform.position = transform.position;
        }
    }
    //____________________________________________________________________________________________________________________________


    //___Gizmos_________________________________________________________________________________________________________________________
    //____________________________________________________________________________________________________________________________


    //___関数_________________________________________________________________________________________________________________________
    private void OnDestroy()
    {
        //爆破生成
    }
    //____________________________________________________________________________________________________________________________
}