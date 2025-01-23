using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static EnemyBase;

public class TutorialMonitorManager : MonoBehaviour
{
    public float interval = 3f;
    public float dissolveDuration = 0.75f;

    public EnemyMaterial screen1;
    public EnemyMaterial screen2;

    private bool currentScreenIs1 = false;
    private float randomValue = 0f;

    public VideoPlayer player1;
    public VideoPlayer player2;

    public VideoClip[] array1;
    public VideoClip[] array2;

    private int index = -1;
    void Start()
    {
        index = -1;

        screen1.InstantiateMaterial();
        screen2.InstantiateMaterial();

        //ChangeScreen();
        IncrementIndex();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            IncrementIndex();
        }
    }

    private void IncrementIndex()
    {
        index++;
        player1.clip = array1[index];
        player2.clip = array2[index];
    }
    private void ChangeScreen()
    {
        if (currentScreenIs1 == false)
        {
            currentScreenIs1 = true;

            screen1.renderer.transform.localScale = 0.99f * Vector3.one;
            screen2.renderer.transform.localScale = 1.00f * Vector3.one;

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen2.DissolveOut(dissolveDuration));
        }
        else
        {
            currentScreenIs1 = false;

            screen1.renderer.transform.localScale = 1.00f * Vector3.one;
            screen2.renderer.transform.localScale = 0.99f * Vector3.one;

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen1.DissolveOut(dissolveDuration));
        }

        randomValue = Random.Range(interval * 0.5f, interval * 1.5f);
        Invoke("ChangeScreen", randomValue);
    }
}
