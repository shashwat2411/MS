using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtkEffect
{
    public void Initiate(float lifetime = 0.8f, float damage = 1.0f);

    public void LevelUp();
} 

