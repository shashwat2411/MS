using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

//[ExecuteAlways]
public class PlayerExp : MonoBehaviour
{
    [Header("Exp")]
    public float exp = 30f;
    public float maxExp = 100f;
    public float shiftSpeed = 0.3f;

    public AnimationCurve nextExpCurve;
    
    [Header("Audio")]
   
    public string levelupSE;
    public string collectExpSE;
    public float collectExpCDTime = 2f;
    bool canPlayCollectExp = true;  

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
    private TextMeshProUGUI ExpText;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();

        shiftBar = transform.GetChild(0).gameObject.GetComponent<Image>();
        baseBar = transform.GetChild(1).gameObject.GetComponent<Image>();
        //borderBar = transform.GetChild(0).gameObject.GetComponent<Image>();
        //innerBorderBar = transform.GetChild(3).gameObject.GetComponent<Image>();
        ExpText = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        UpdateColor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ExpFill(3f);
        }

        exp = player.playerData.exp;
        maxExp = player.playerData.nextExp;

        shiftBar.fillAmount = exp / maxExp;
        baseBar.fillAmount = Mathf.Lerp(baseBar.fillAmount, shiftBar.fillAmount, shiftSpeed);

        ExpText.text = exp.ToString("N0");
        Debug.Log("exp:" + exp + ":::::maxexp:" + maxExp + "::::fill:" + baseBar.fillAmount);
    }

    public void ExpFill(float value)
    {
        float result = exp + value;

        if(canPlayCollectExp)
        {
            StartCoroutine(PlayCollectExpSE());
        }

        if (result < maxExp) { exp = result; }
        else
        {
            exp = result - maxExp;


            SoundManager.Instance.PlaySE(levelupSE);

            FindFirstObjectByType<SkillSelect>().LevelUp();
            player.playerData.nextExp = maxExp * 1.2f;
        }

        player.playerData.exp = exp;
    }


    IEnumerator PlayCollectExpSE()
    {
        SoundManager.Instance.PlaySE(collectExpSE);
        canPlayCollectExp = false;
     
        yield return new WaitForSeconds(collectExpCDTime);

        canPlayCollectExp = true;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateColor();
    }
#endif

    private void UpdateColor()
    {
        //if (baseBar != null) { baseBar.color = baseColor; }
        //if (shiftBar != null) { shiftBar.color = shiftColor; }
        //if (borderBar != null) { borderBar.color = borderColor; }
        //if (innerBorderBar != null) { innerBorderBar.color = borderColor; }
    }
}
