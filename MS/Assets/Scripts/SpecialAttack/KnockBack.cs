using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class KnockBack : MonoBehaviour
{
    static float factor = 1.0f;

    [SerializeField] private float damage;
    [SerializeField] private float knockForce = 3000.0f;

    private Animator animator;
    private VisualEffect sparkParticles;

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        sparkParticles = GetComponentInChildren<VisualEffect>();
        animator = GetComponent<Animator>();
    }

    public void LevelUp()
    {
        factor *= 1.2f;
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject);
        }
    }

    private void StartExplosion()
    {
        sparkParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if(enemy)
        {
            Vector3 direction = enemy.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0f;

            //enemy.GetComponent<Rigidbody>().AddForce(direction * knockForce * factor);
            enemy.Knockback(direction, knockForce * factor);
            enemy.Damage(damage * factor);
        }
    }
}
