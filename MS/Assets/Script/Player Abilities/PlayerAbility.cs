using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    protected PlayerManager player;
    protected virtual void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
    }

    protected virtual void FixedUpdate()
    {
        
    }
}
