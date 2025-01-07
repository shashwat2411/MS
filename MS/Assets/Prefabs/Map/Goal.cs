using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Goal : MonoBehaviour
{
    public VisualEffect vortex;
    public BoxCollider box;

    private bool once = false;
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();

        vortex.Stop();
        box.gameObject.SetActive(false);
        once = false;
    }
    private void FixedUpdate()
    {
        if (enemyManager)
        {
            if (enemyManager.GetCount() <= 0 && once == false)
            {
                once = true;
                vortex.Play();
                box.gameObject.SetActive(true);
            }
        }
    }
}
