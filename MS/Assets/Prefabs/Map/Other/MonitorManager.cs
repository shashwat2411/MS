using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBase;

public class MonitorManager : MonoBehaviour
{
    public float interval = 3f;
    public float dissolveDuration = 0.75f;
    public float commentSpeed = 1f;

    [Header("Movement")]
    public AnimationCurve movementCurve;
    public float displacement;
    private float curve = 0f;
    private float movementSpeed = 1f;

    public EnemyMaterial screen1;
    public EnemyMaterial screen2;

    private bool currentScreenIs1 = false;
    private float randomValue = 0f;
    private float offset = 0f;

    //Hash Map
    private int _Offset = Shader.PropertyToID("_Offset");

    private void Start()
    {
        commentSpeed *= Random.Range(-1.2f, 1.2f);
        movementSpeed *= Random.Range(0.1f, 1f);
        displacement *= Random.Range(-1.2f, 1.2f);

        offset = 0f;
        curve = 0f;

        screen1.InstantiateMaterial();
        screen2.InstantiateMaterial();

        ChangeScreen();
    }

    private void FixedUpdate()
    {
        if (offset < 1f) { offset += Time.deltaTime * commentSpeed; }
        else { offset -= 1f; }
        screen1.material.SetVector(_Offset, new Vector4(-offset, 0f, 0f, 0f));

        if (curve < 1f) { curve += Time.deltaTime * movementSpeed; }
        else { curve -= 1f; }
        float y = movementCurve.Evaluate(curve);
        Vector3 position = transform.GetChild(0).transform.localPosition;
        position.y = y * displacement;
        transform.GetChild(0).localPosition = position;
    }

    private void ChangeScreen()
    {
        if(currentScreenIs1 == false)
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
