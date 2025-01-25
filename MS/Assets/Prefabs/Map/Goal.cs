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

    public bool debug = false;



    private void Start()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();

        if (debug == false)
        {
            vortex.Stop();
            box.gameObject.SetActive(false);
            once = false;
        }
        else
        {
            EnableGoal();
        }
    }
    private void FixedUpdate()
    {
        if (enemyManager)
        {
            if (enemyManager.GetCount() <= 0 && once == false)
            {
                once = true;
                EnableGoal();

                // Player Bonus
                FindFirstObjectByType<SkillSelect>().LevelUp();
               
            }
        }
    }

    public void EnableGoal()
    {
        once = true;
        vortex.Play();
        box.gameObject.SetActive(true);
    }
}
