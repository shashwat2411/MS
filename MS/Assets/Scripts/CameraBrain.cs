using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    public float smoothTime = 0.25f;
    public float rotationAngle = 20f;

    private float shakeIntensity = 0.8f;
    private float shakeTime = 0.2f;

    public Vector3 offset;
    public Vector3 angleOffset;
    public Vector3 zoomInOffset;
    public Vector3 zoomOutOffset;
    private Vector3 originalOffset;
    private Vector3 velocity = Vector3.zero;

    public Vector3 zoomInFactor;
    public Vector3 direction;

    private PlayerManager player;
    [SerializeField] private TeleportOutCutScene teleportOut;

    public AnimationCurve zoomInY;
    public AnimationCurve zoomInZ;
    private float zoomInCounter = 0f;
    public bool zoomIn = false;
    void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();

        originalOffset = offset;
        zoomInCounter = 0f;
        zoomIn = false;
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(player.transform.position + angleOffset);

        float value = player.GetMovementInput().x;
        if(value > 0.3f)
        {
            transform.parent.RotateAround(player.transform.position, new Vector3(0, 1, 0), rotationAngle * value * Time.deltaTime);
        }
        else if (value < 0.3f)
        {
            transform.parent.RotateAround(player.transform.position, new Vector3(0, 1, 0), -rotationAngle * value * Time.deltaTime);
        }

        if(zoomIn == true)
        {
            if (zoomInCounter < 1f) 
            { 
                zoomInCounter += Time.deltaTime;

                float y = zoomInY.Evaluate(zoomInCounter);
                float z = zoomInZ.Evaluate(zoomInCounter);

                Vector3 position = transform.localPosition;
                direction = (player.transform.position - transform.localPosition).normalized;
                Vector3 displacement = new Vector3(0f, direction.y * y * zoomInFactor.y, direction.z * z * zoomInFactor.z);

                position += displacement;

                transform.localPosition = position;
            }
            else { zoomInCounter = 1f; }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { StartCoroutine(CameraShake(shakeTime, shakeIntensity)); }
        if (Input.GetKeyDown(KeyCode.K)) { StartCoroutine(ZoomIn(5f)); }
        if (Input.GetKeyDown(KeyCode.J)) { StartCoroutine(ZoomOut(5f)); }
    }

    public void ZoomInTrigger()
    {
        zoomIn = true;
    }

    public void TriggerPlayerDissolveOut()
    {
        teleportOut.TriggerPlayerDissolveOut();
    }
    public void TriggerPlayerDissolveIn()
    {
        teleportOut.TriggerPlayerDissolveIn();
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        GetComponent<Animator>().enabled = false;

        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (Time.timeScale > 0)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, originalPosition.z);

                elapsed += Time.unscaledDeltaTime;

                if (zoomIn == true) { elapsed = duration; }
            }

            yield return null;
        }

        transform.localPosition = originalPosition;

        GetComponent<Animator>().enabled = false;
    }
    public IEnumerator ZoomIn(float time)
    {
        offset = Vector3.Lerp(offset, zoomInOffset, 0.5f);

        yield return new WaitForSeconds(time);

        yield return StartCoroutine(NormalZoom(2f));
    }
    public IEnumerator ZoomOut(float time)
    {
        offset = Vector3.Lerp(offset, zoomOutOffset, 0.5f);

        yield return new WaitForSeconds(time);

        yield return StartCoroutine(NormalZoom(2f));
    }
    public IEnumerator NormalZoom(float time)
    {
        offset = Vector3.Lerp(offset, originalOffset, 0.5f);

        yield return new WaitForSeconds(time);
    }
}
