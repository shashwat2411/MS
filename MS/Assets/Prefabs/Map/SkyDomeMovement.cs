using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDomeMovement : MonoBehaviour
{
    public Vector3 rotationValue;
    void FixedUpdate()
    {
        transform.Rotate(rotationValue);
    }
}
