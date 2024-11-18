using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableEnemyObject : MonoBehaviour
{
    protected float motionTime;
    public float timeForImpact;

    protected Vector3 target;
    protected Vector3 startPosition;

    public AnimationCurve motion;
    protected GameObject player;
    protected GameObject owner;
    virtual protected void Start()
    {
        motionTime = 0f;
        startPosition = transform.position;
        player = FindFirstObjectByType<PlayerManager>().gameObject;
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {
        if (motionTime < 1.0f) { motionTime += Time.deltaTime / timeForImpact; }
        else { motionTime = 1.0f; }

        float y = motion.Evaluate(motionTime);

        Vector3 position = Vector3.Lerp(startPosition, target, motionTime);
        position.y = position.y + y;
        transform.position = position;
    }
    virtual protected void OnDestroy()
    {

    }

    public void SetTarget(Vector3 value) { target = value; }
    public void SetOwner(GameObject value) { owner = value; }
}
