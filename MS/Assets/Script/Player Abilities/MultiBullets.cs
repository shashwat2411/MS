using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBullets : PlayerAbility
{
    PlayerAttack playerAttack;
    protected override void Start()
    {
        base.Start();
        playerAttack = player.GetComponent<PlayerAttack>();
        
    }
}
