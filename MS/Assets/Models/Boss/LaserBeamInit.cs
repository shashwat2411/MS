using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamInit : MonoBehaviour
{

    public ParticleSystem laserBeam;
    public GameObject colider;
    public BossEnemy boss;



    public float hitTime = 0.2f;

    [Header("For Test")]
    public bool laserOn;

    // Start is called before the first frame update
    void Awake()
    {
        colider.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (laserOn)
        {
            //StartCoroutine(IniteLaser());
            laserOn = false;

        }
    }

    IEnumerator IniteLaser()
    {
        laserBeam.gameObject.SetActive(true);
        laserBeam.Play();

        yield return new WaitForSeconds(1.0f);
        colider.gameObject.SetActive(true);


        yield return new WaitForSecondsRealtime(hitTime);

        colider.gameObject.SetActive(false);


        yield return new WaitForSecondsRealtime(0.2f);
        laserBeam.Stop();
        laserBeam.gameObject.SetActive(false);


    }
}