using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public ThrowEnemy owner;


    public void RotateEnable()
    {
        owner.gameObject.GetComponent<MegaphoneEnemy>().SetStopRotation(false);
    }    
    public void RotateDisable()
    {
        owner.gameObject.GetComponent<MegaphoneEnemy>().SetStopRotation(true);
    }
    public void Attack()
    {
        owner.AttackInstantiate();
    }

    public void AttackOver()
    {
        owner.AttackOver();
    }
}
