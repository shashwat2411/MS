using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDashAbility : PlayerAbility
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player.gameObject.GetComponent<PlayerDash>().LevelUp();
    }
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject);
    }


}
