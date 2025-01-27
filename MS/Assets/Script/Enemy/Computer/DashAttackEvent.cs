using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackEvent : MonoBehaviour
{
    public DashEnemy owner;

    public void ChangeToAttack()
    {
        owner.ChangeToAttack();
    }
    public void Dash()
    {
        owner.InitiateDash();
    }
    public void AttackOver()
    {
        owner.AttackOver();
    }
    public void PlaySound()
    {
        SoundManager.Instance.PlaySE("EnemyDash");
    }
}
