using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Video;
using static EnemyBase;

public class TutorialMonitorManager : MonoBehaviour
{
    private bool currentScreenIs1 = true;
    public float dissolveDuration = 0.75f;

    public EnemyMaterial screen1;
    public EnemyMaterial screen2;

    public VideoPlayer player1;
    public VideoPlayer player2;

    public VideoClip[] array1;
    public VideoClip[] array2;

    private int index1 = -1;
    private int index2 = -1;

    public float backZ = 0.061f;
    public float frontZ = 0.078f;
    void Start()
    {
        index1 = -1;
        index2 = -1;

        screen1.InstantiateMaterial();
        screen2.InstantiateMaterial();

        //ChangeScreen();
        IncrementIndex1();
        IncrementIndex2();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            IncrementIndex1();
            IncrementIndex2();
        }
    }

    private void IncrementIndex1()
    {
        if (index1 < (array1.Length - 1)) { index1++; }
        else { index1 = array1.Length - 1; }

        player1.clip = array1[index1];
    }
    private void IncrementIndex2()
    {
        if (index2 < (array2.Length - 1)) { index2++; }
        else { index2 = array2.Length - 1; }

        player2.clip = array2[index2];
    }
    public void ChangeScreen()
    {
        if (currentScreenIs1 == false)
        {
            currentScreenIs1 = true;

            SetLocalPosition(screen1.renderer.transform, backZ);
            SetLocalPosition(screen2.renderer.transform, frontZ);
            //screen2.renderer.transform.localScale = 1.00f * Vector3.one;

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen2.DissolveOut(dissolveDuration));
            Invoke("IncrementIndex2", dissolveDuration);
        }
        else
        {
            currentScreenIs1 = false;

            SetLocalPosition(screen1.renderer.transform, frontZ);
            SetLocalPosition(screen2.renderer.transform, backZ);
            //screen1.renderer.transform.localScale = 1.00f * Vector3.one;
            //screen2.renderer.transform.localScale = 0.99f * Vector3.one;

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen1.DissolveOut(dissolveDuration));
            Invoke("IncrementIndex1", dissolveDuration);
        }
    }

    private void SetLocalPosition(Transform point, float position)
    {
        Vector3 localPosition = point.localPosition;
        localPosition.z = position;
        point.localPosition = localPosition;
    }
}
