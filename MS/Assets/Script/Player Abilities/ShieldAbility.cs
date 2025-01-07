using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class ShieldAbility : PlayerAbility
{
    public float rotationSpeed;
    public AnimationCurve appearAnimation;

    private int orderNumber;
    private float appearTime;
    private Vector3 localScale;
    private Material shield;

    //Default Values
    private float alpha;
    [ColorUsage(false, true)] private Color frontColor;
    [ColorUsage(false, true)] private Color backColor;
    [ColorUsage(false, true)] private Color fresnelColor;

    //Set Values
    public float speed;
    public float hitAlpha;
    [ColorUsage(false, true)] public Color hitFrontColor;
    [ColorUsage(false, true)] public Color hitBackColor;
    [ColorUsage(false, true)] public Color hitFesnelColor;

    //Hash Maps
    private int _alpha = Shader.PropertyToID("_Alpha");
    private int _frontColor = Shader.PropertyToID("_FrontColor");
    private int _backColor = Shader.PropertyToID("_BackColor");
    private int _fresnelColor = Shader.PropertyToID("_FresnelColor");

    protected override void Start()
    {
        base.Start();

        orderNumber = 0;
        shield = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        //Set
        alpha = shield.GetFloat(_alpha);
        frontColor = shield.GetColor(_frontColor);
        backColor = shield.GetColor(_backColor);
        fresnelColor = shield.GetColor(_fresnelColor);

        //Scale
        appearTime = 0f;
        localScale = transform.localScale;

        float value = appearAnimation.Evaluate(appearTime);
        transform.localScale = localScale * value;

        CalculateOrder();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //Position
        transform.position = player.transform.position;


        //Rotation
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);


        //Scale
        if (appearTime < 1f) { appearTime += Time.deltaTime; }
        else { appearTime = 1f; }


        float value = appearAnimation.Evaluate(appearTime);
        transform.localScale = localScale * value;


        //Set
        shield.SetFloat(_alpha, Mathf.Lerp(shield.GetFloat(_alpha), alpha, speed));
        shield.SetColor(_frontColor, Color.Lerp(shield.GetColor(_frontColor), frontColor, speed));
        shield.SetColor(_backColor, Color.Lerp(shield.GetColor(_backColor), backColor, speed));
        shield.SetColor(_fresnelColor, Color.Lerp(shield.GetColor(_fresnelColor), fresnelColor, speed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        ThrowableEnemyObject thrownObject = collision.gameObject.GetComponent<ThrowableEnemyObject>();

        if (thrownObject != null)
        {
            thrownObject.SetTarget(thrownObject.GetOwner().transform.position);
            thrownObject.SetPlayer(thrownObject.GetOwner());
            thrownObject.SetOwner(player.gameObject);
            thrownObject.ResetMotion();


            shield.SetFloat(_alpha, hitAlpha);
            shield.SetColor(_frontColor, hitFrontColor);
            shield.SetColor(_backColor, hitBackColor);
            shield.SetColor(_fresnelColor, hitFesnelColor);
        }
    }

    private void CalculateOrder()
    {
        ShieldAbility[] shields = FindObjectsOfType<ShieldAbility>();

        orderNumber = shields.Length;

        switch (orderNumber)
        {
            case 1:
                {
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    transform.rotation = quaternion;

                    break;
                }

            case 2:
                {
                    Quaternion angle1 = shields[1].gameObject.transform.rotation;

                    Quaternion quaternion = Quaternion.Euler(angle1.eulerAngles + new Vector3(0f, 180f, 0f));
                    shields[0].gameObject.transform.rotation = quaternion;

                    break;
                }

            case 3:
                {
                    Quaternion angle1 = shields[1].gameObject.transform.rotation;

                    Quaternion quaternion1 = Quaternion.Euler(angle1.eulerAngles + new Vector3(0f, 120f, 0f));
                    Quaternion quaternion2 = Quaternion.Euler(angle1.eulerAngles + new Vector3(0f, 240f, 0f));

                    shields[2].gameObject.transform.rotation = quaternion1;
                    shields[0].gameObject.transform.rotation = quaternion2;

                    break;
                }

            default:
                transform.rotation = Quaternion.identity;
                break;
        }
    }
}
