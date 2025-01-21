using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenShatter : MonoBehaviour
{
    public float transitionSpeed = 0.002f;
    public Canvas mainCanvas;

    public Camera mainCamera;
    public Camera captureCamera;
    public Camera shatterCamera;
    public GameObject plane;

    public Animator[] shards;

    private bool reset;
    private bool fadeIn;

    public string loadLevel;

    private void Awake()
    {
        //System.Array.Sort(shards, (a, b) => a.transform.localPosition.x.CompareTo(b.transform.localPosition.x));

        StartCoroutine(DelayCall());
    }
    private IEnumerator DelayCall()
    {
        mainCamera.farClipPlane = 0f;

        fadeIn = true;
        ResetScreen();
        plane.SetActive(true);

        yield return null;

        captureCamera.gameObject.SetActive(true);
        mainCanvas.worldCamera = captureCamera;

        yield return ShatterReverseScreenInitate();
    }

    private void Update()
    {
        captureCamera.fieldOfView = mainCamera.fieldOfView;

        if(fadeIn == true)
        {
            if (shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).IsName("shatterReverse") && shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                fadeIn = false;
                ResetScreen();
                plane.SetActive(false);
            }
        }
        else
        {
            if (shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).IsName("shatter") && shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                SceneManager.LoadScene(loadLevel);
            }
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(ShatterScreenInitate());
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(DelayCall());
        }
    }

    public IEnumerator ShatterScreenInitate()
    {
        plane.SetActive(true);
        reset = false;
        captureCamera.gameObject.SetActive(true);
        mainCanvas.worldCamera = captureCamera;

        yield return null;

        mainCamera.gameObject.SetActive(false);
        captureCamera.gameObject.SetActive(false);
        shatterCamera.gameObject.SetActive(true);

        yield return ShatterCoroutine();
    }
    private IEnumerator ShatterCoroutine()
    {
        for (int i = 0; i < shards.Length; i++)
        {
            if (reset == true)
            {
                i = shards.Length;
            }
            else
            {
                //shards[i].SetBool("in", false);
                //shards[i].SetBool("out", true);
                shards[i].Play("shatter", -1, 0f);
                yield return new WaitForSeconds(transitionSpeed);
            }
        }
    }

    private IEnumerator ShatterReverseScreenInitate()
    {
        reset = false;
        yield return null;

        //mainCamera.gameObject.SetActive(false);
        mainCamera.enabled = false;
        mainCamera.farClipPlane = 1000f;
        //captureCamera.gameObject.SetActive(false);
        shatterCamera.gameObject.SetActive(true);

        yield return ShatterReverseCoroutine();
    }

    private IEnumerator ShatterReverseCoroutine()
    {
        for (int i = 0; i < shards.Length; i++)
        {
            if (reset == true)
            {
                i = shards.Length;
            }
            else
            {
                shards[i].Play("shatterReverse", -1, 0f);
                yield return new WaitForSeconds(transitionSpeed);
            }
        }
    }

    private void ResetScreen()
    {
        reset = true;

        mainCamera.gameObject.SetActive(true);
        mainCamera.enabled = true;
        captureCamera.gameObject.SetActive(false);
        shatterCamera.gameObject.SetActive(false);

        mainCanvas.worldCamera = mainCamera;
    }
}
