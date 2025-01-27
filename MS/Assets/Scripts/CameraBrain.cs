using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UniSense;

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
    [HideInInspector] public Transform target;
    [SerializeField] private TeleportOutCutScene teleportOut;

    public AnimationCurve zoomInY;
    public AnimationCurve zoomInZ;
    private float zoomInCounter = 0f;
    public bool zoomIn = false;
    private bool targetTransition = false;

    [Header("RumbleTest")]
    [SerializeField]
    [Range(1, 50)]
    float DamgeTest;


    void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();
        target = player.transform;

        originalOffset = offset;
        zoomInCounter = 0f;
        zoomIn = false;
        targetTransition = false;
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, targetPosition, ref velocity, smoothTime);

        if (targetTransition == false) { transform.LookAt(target.transform.position + angleOffset); }

        float value = player.GetMovementInput().x;
        if (value > 0.3f)
        {
            transform.parent.RotateAround(target.transform.position, new Vector3(0, 1, 0), rotationAngle * value * Time.deltaTime);
        }
        else if (value < 0.3f)
        {
            transform.parent.RotateAround(target.transform.position, new Vector3(0, 1, 0), -rotationAngle * value * Time.deltaTime);
        }

        if (zoomIn == true)
        {
            if (zoomInCounter < 1f)
            {
                zoomInCounter += Time.deltaTime;

                float y = zoomInY.Evaluate(zoomInCounter);
                float z = zoomInZ.Evaluate(zoomInCounter);

                Vector3 position = transform.localPosition;
                direction = (target.transform.position - transform.localPosition).normalized;
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

        //  Rumble(test)
        if (Gamepad.current?.buttonWest.isPressed == true)  
        {
            //SetRumbleValue(DamgeTest, true);
           player.playerHP.Damage(20.0f);
           
           //SetGamePadMotorSpeed(DamgeTest, true);
        }
        
    }


    public IEnumerator ShiftTarget(Transform nextTarget, float duration)
    {
        float elapsed = 0f;
        targetTransition = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            Vector3 position = Vector3.Lerp(target.transform.position, nextTarget.position, elapsed / duration) + angleOffset;
            transform.LookAt(position);

            yield return null;
        }

        target = nextTarget;
        targetTransition = false;
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

           
            SetGamePadMotorSpeed(magnitude, true);
            yield return null;
        }

        transform.localPosition = originalPosition;

        SetGamePadMotorSpeed(0.0f, false);

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


    void SetGamePadMotorSpeed(float magnitude, bool use)
    {
        Vector2 motorspeed;

        float value = magnitude * 20.0f;

        motorspeed.y = value * value / (50.0f * 50.0f);
        motorspeed.x = value / 50.0f;

        //motorspeed.Normalize();

        if (motorspeed.x > 1.0f) { motorspeed.x = 1.0f; }
        if (motorspeed.y > 1.0f) { motorspeed.y = 1.0f; }

        Debug.Log(magnitude + "::::::" + motorspeed.x + ":::::" + motorspeed.y);

        if (use == true)
        {
            Gamepad.current?.SetMotorSpeeds(motorspeed.x, motorspeed.y);
        }
        else
        {
            Gamepad.current?.SetMotorSpeeds(0.0f, 0.0f);
        }

    }

}