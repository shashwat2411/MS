using System.Collections;
using System.IO;
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

    public ShardAnimation[] shards;

    private bool reset;
    private bool fadeIn;

    public string loadLevel;

    public AnimationCurve shardShatterAnimationCurve;
    public AnimationCurve shardShatterReverseAnimationCurve;
    public float animationDuration;
    public float finalPositionZ;

    private PostProcessController postProcess;
    public float lensScale = 0.95f;


    private void Awake()
    {
        postProcess = FindFirstObjectByType<PostProcessController>();

        ShuffleArray(shards);
        StartCoroutine(DelayCall());
    }

    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1); // Pick a random index
            T temp = array[i]; // Swap the elements
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private IEnumerator DelayCall()
    {
        postProcess.lensDistortionScale = lensScale;
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
            //if (shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).IsName("shatterReverse") && shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            //{
            //    fadeIn = false;
            //    ResetScreen();
            //    plane.SetActive(false);
            //}
            if (shards[shards.Length - 1].reverseAnimation == true)
            {
                fadeIn = false;
                ResetScreen();
                postProcess.lensDistortionScale = 1f;
                plane.SetActive(false);
            }
        }
        else
        {
            //if (shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).IsName("shatter") && shards[shards.Length - 1].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            //{
            //    SceneManager.LoadScene(loadLevel);
            //}
            if (shards[shards.Length - 1].animation == true)
            {
                Time.timeScale = 1f;
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


    public IEnumerator FailShatterScreenInitate()
    {
        loadLevel = "MainMenu";

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
                //shards[i].Play("shatter", -1, 0f);
                StartCoroutine(shards[i].Shatter(animationDuration));
                yield return new WaitForSecondsRealtime(transitionSpeed);
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
                //shards[i].Play("shatterReverse", -1, 0f);
                StartCoroutine(shards[i].ShatterReverse(animationDuration));
                yield return new WaitForSecondsRealtime(transitionSpeed);
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
