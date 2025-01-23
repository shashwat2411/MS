using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static EnemyBase;

public class TutorialTextManager : MonoBehaviour
{
    public EnemyMaterial screen1;
    public EnemyMaterial screen2;
    void Start()
    {
        screen1.InstantiateMaterial();
        screen2.InstantiateMaterial();
    }
}
