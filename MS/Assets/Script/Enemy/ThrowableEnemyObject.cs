using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ThrowableEnemyObject : MonoBehaviour
{
    protected bool moveOn;
    public bool moveInFacedDirection = false;

    protected float damage;
    protected float motionTime;
    protected float lifetime;
    protected float maxLifetime;
    public float timeForImpact;
    public float speed; //moveOn Ç™falseÇÃèÍçáÇæÇØ

    protected Vector3 target;
    public Vector3 offset;
    protected Vector3 startPosition;
    public Vector3 direction;

    public AnimationCurve motion;
    protected GameObject player;
    protected GameObject owner;
    virtual protected void Start()
    {
        moveOn = false;

        motionTime = 0f;
        lifetime = 0f;

        startPosition = transform.position;
        player = FindFirstObjectByType<PlayerManager>().gameObject;

        if (moveInFacedDirection == false)
        {
            Vector3 position = Vector3.Lerp(startPosition, target + offset, Time.deltaTime / timeForImpact);
            direction = (position - transform.position).normalized;
            direction.y = 0f;
        }
        else
        {
            direction = transform.forward;
            direction.y = 0f;
        }
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {
        if (motionTime < 1.0f) { motionTime += Time.deltaTime / timeForImpact; }
        else { motionTime = 1.0f; }

        if (lifetime < maxLifetime) { lifetime += Time.deltaTime; }
        else { Destroy(gameObject); }

        float y = motion.Evaluate(motionTime);

        if (motionTime <= 1f)
        {
            if (moveOn == false)
            {
                Vector3 position = Vector3.Lerp(startPosition, target + offset, motionTime);
                position.y = position.y + y;
                transform.position = position;
            }
            else
            {
                Vector3 position = transform.position + speed * direction;
                transform.position = position;
            }
        }
    }
    virtual protected void OnDestroy()
    {

    }

    public void ResetMotion()
    {
        motionTime = 0f;
        lifetime = 0f;
        startPosition = transform.position;
        direction = -direction;
    }

    public void SetMaxLifetime(float value) { maxLifetime = value; }
    public void SetDamage(float value) { damage = value; }
    public void SetTarget(Vector3 value) { target = value; }
    public void SetOwner(GameObject value) { owner = value; }
    public void SetPlayer(GameObject value) { player = value; }

    public GameObject GetOwner() { return owner; }
}
