using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundFire : MonoBehaviour
{
    List<GameObject> allEnemies = new List<GameObject>();

    static float factor = 5.0f;
    private float curTime = 0;

    public float damage;
    public float damageInterval = 0.6f;

    [Header("ParticleEffectReference")]
    public ParticleSystem groundMark_01;
    public ParticleSystem groundMark_02;
    public ParticleSystem groundMark_03;

    [Header("SE")]
    public string groundFireSE;

    private PlayerManager player;

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f, float declineInterval = 1.0f, Transform usedMenko = null)
    {
        lifetime = factor * declineInterval;

        ChangeDuration(groundMark_01, lifetime);
        ChangeDuration(groundMark_02, lifetime);
        ChangeDuration(groundMark_02, lifetime);

        player = FindFirstObjectByType<PlayerManager>();

        float scale = (player.playerData.charge + 1f) / player.playerData.maxChargeTime;
        float finalScale = Mathf.Lerp(1.0f, 1.8f, scale);

        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            if (system != null)
            {
                system.Play();
                Vector3 localScale = system.transform.localScale;
                system.transform.localScale = localScale * finalScale;
            }
        }

        Destroy(gameObject, lifetime);


        this.damage = damage;
    }
    void FixedUpdate()
    {
        curTime += Time.deltaTime;
        if (curTime >= damageInterval)
        {
            DamageAllEnemies();
            curTime = 0;
        }
    }

    private void Awake()
    {
        SoundManager.Instance.PlaySE(groundFireSE);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy && !allEnemies.Contains(other.gameObject))
        {
            allEnemies.Add(enemy.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy && allEnemies.Contains(other.gameObject))
        {
            allEnemies.Remove(enemy.gameObject);
        }
    }

    void DamageAllEnemies()
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            if (allEnemies[i] != null)
            {
                var enemyBase = allEnemies[i].GetComponent<EnemyBase>();
                if (enemyBase.dead)
                {
                    allEnemies.Remove(allEnemies[i]);
                }
                else
                {
                    enemyBase.Damage(this.damage);
                }
            }
        }
    }

    private void ChangeDuration(ParticleSystem system, float lifetime)
    {
        ParticleSystem.MainModule main = system.main;

        main.duration = lifetime;
        main.startLifetime = lifetime * 0.95f;
    }
}
