using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[ExecuteAlways]
public class PlayerExp : MonoBehaviour
{
    [Header("Exp")]
    public float exp = 100f;
    public float maxExp = 100f;
    public float shiftSpeed = 0.3f;

    public AnimationCurve nextExpCurve;


    [Header("Color")]
    public Color baseColor = Color.white.WithAlpha(0f);
    public Color shiftColor = Color.red;
    public Color borderColor = Color.black;


    [Header("References")]
    private Image baseBar;
    private Image shiftBar;
    private Image borderBar;
    private Image innerBorderBar;
    private PlayerManager player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();

        baseBar = transform.GetChild(2).gameObject.GetComponent<Image>();
        shiftBar = transform.GetChild(1).gameObject.GetComponent<Image>();
        borderBar = transform.GetChild(0).gameObject.GetComponent<Image>();
        innerBorderBar = transform.GetChild(3).gameObject.GetComponent<Image>();
        UpdateColor();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            ExpFill(30f);
        }
    }
    void FixedUpdate()
    {
        exp = player.playerData.exp;
        maxExp = player.playerData.nextExp;

        baseBar.fillAmount = exp / maxExp * 0.5f;
        shiftBar.fillAmount = Mathf.Lerp(shiftBar.fillAmount, baseBar.fillAmount, shiftSpeed);
    }

    public void ExpFill(float value)
    {
        float result = exp + value;

        if (result < maxExp) { exp = result; }
        else
        {
            exp = result - maxExp;

            FindFirstObjectByType<SkillSelect>().LevelUp();
            player.playerData.nextExp = maxExp * 1.2f;
        }

        player.playerData.exp = exp;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateColor();
    }
#endif

    private void UpdateColor()
    {
        if (baseBar != null) { baseBar.color = baseColor.WithAlpha(0f); }
        if (shiftBar != null) { shiftBar.color = shiftColor; }
        if (borderBar != null) { borderBar.color = borderColor; }
        if (innerBorderBar != null) { innerBorderBar.color = borderColor; }
    }
}
