using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

//[ExecuteAlways]
public class PlayerMP : MonoBehaviour
{
    [Header("MP")]
    public float mp = 100f;
    public float maxMp = 100f;
    public float shiftSpeed = 0.3f;

    public AnimationCurve nextExpCurve;


    [Header("Color")]
    public Color baseColor = Color.white;
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

    void FixedUpdate()
    {
        mp = player.playerData.mp;
        maxMp = player.playerData.maxMp;

        baseBar.fillAmount = mp / maxMp * 0.5f;
        shiftBar.fillAmount = Mathf.Lerp(shiftBar.fillAmount, baseBar.fillAmount, shiftSpeed);
    }

    public void ExpendMP(float value)
    {
        float result = mp - value;

        if (result > 0f) { mp = result; }
        else
        {
            mp = 0f;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateColor();
    }
#endif

    private void UpdateColor()
    {
        if (baseBar != null) { baseBar.color = baseColor; }
        if (shiftBar != null) { shiftBar.color = shiftColor; }
        if (borderBar != null) { borderBar.color = borderColor; }
        if (innerBorderBar != null) { innerBorderBar.color = borderColor; }
    }
}
