using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    private float motionTime;
    public float timeForImpact;

    private Vector3 target;
    private Vector3 startPosition;
    
    public AnimationCurve motion;
    void Start()
    {
        motionTime = 0f;
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (motionTime < 1.0f) { motionTime += Time.deltaTime / timeForImpact; }
        else { motionTime = 1.0f; }

        float y = motion.Evaluate(motionTime);

        Vector3 position = Vector3.Lerp(startPosition, target, motionTime);
        position.y = position.y + y;
        transform.position = position;
    }

    private void OnDestroy()
    {
        //”š•—
    }

    public void SetTarget(Vector3 value) { target = value; }

}
