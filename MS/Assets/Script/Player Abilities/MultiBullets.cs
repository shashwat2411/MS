using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBullets : PlayerAbility
{
    PlayerAttack playerAttack;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        playerAttack = player.GetComponent<PlayerAttack>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
