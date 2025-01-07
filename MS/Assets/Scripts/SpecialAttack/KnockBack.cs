using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    static float factor = 1.0f;

    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float size;
    [SerializeField] private float knockForce = 3000.0f;

    public AnimationCurve colliderGrowth;
    private float counter = 0f;

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f)
    {
        Destroy(gameObject, this.lifetime * factor);

        transform.parent.localScale = size * factor * Vector3.one;
        counter = 0f;

        transform.localScale = Vector3.zero;
    }

    public void LevelUp()
    {
        factor *= 1.2f;
    }

    private void FixedUpdate()
    {
        if (counter < 1f) { counter += Time.deltaTime / (lifetime * factor); }
        else { counter = 1f; }

        float y = colliderGrowth.Evaluate(counter);
        transform.localScale = y * Vector3.one;
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
