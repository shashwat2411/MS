using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class ShieldAbility : PlayerAbility
{
    public float rotationSpeed;
    public AnimationCurve appearAnimation;

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
    protected override void Start()
    {
        base.Start();

        shield = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        alpha = shield.GetFloat("_Alpha");
        frontColor = shield.GetColor("_FrontColor");
        backColor = shield.GetColor("_BackColor");
        fresnelColor = shield.GetColor("_FresnelColor");

        appearTime = 0f;
        localScale = transform.localScale;

        float value = appearAnimation.Evaluate(appearTime);
        transform.localScale = localScale * value;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (appearTime < 1f) { appearTime += Time.deltaTime; }
        else { appearTime = 1f; }

        float value = appearAnimation.Evaluate(appearTime);
        transform.localScale = localScale * value;

        transform.position = player.transform.position;

        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);


        shield.SetFloat("_Alpha", Mathf.Lerp(shield.GetFloat("_Alpha"), alpha, speed));
        shield.SetColor("_FrontColor", Color.Lerp(shield.GetColor("_FrontColor"), frontColor, speed));
        shield.SetColor("_BackColor", Color.Lerp(shield.GetColor("_BackColor"), backColor, speed));
        shield.SetColor("_FresnelColor", Color.Lerp(shield.GetColor("_FresnelColor"), fresnelColor, speed));
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        ThrowableEnemyObject thrownObject = collision.gameObject.GetComponent<ThrowableEnemyObject>();

        if (thrownObject != null)
        {
            thrownObject.SetTarget(thrownObject.GetOwner().transform.position);
            thrownObject.SetPlayer(thrownObject.GetOwner());
            thrownObject.SetOwner(player.gameObject);
            thrownObject.ResetMotion();


            shield.SetFloat("_Alpha", hitAlpha);
            shield.SetColor("_FrontColor", hitFrontColor);
            shield.SetColor("_BackColor", hitBackColor);
            shield.SetColor("_FresnelColor", hitFesnelColor);
        }
    }
}
