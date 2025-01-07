using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    public bool disappear = true;
    private bool damage = false;


    [Header("Disappear")]
    public bool stimulate = false;
    public float appearSpeed; //åªÇÍÇÈë¨ìx
    public float disappearSpeed; //è¡Ç¶ÇÈë¨ìx
    public float disappearTime; //è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘
    private float counter = 0f;


    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float shiftSpeed = 0.3f;


    [Header("Color")]
    public Color baseColor = Color.white;
    public Color shiftColor = Color.red;
    public Color borderColor = Color.black;


    [Header("References")]
    private Image baseBar;
    private Image shiftBar;
    private Image borderBar;

    private void Awake()
    {
        baseBar = transform.GetChild(2).gameObject.GetComponent<Image>();
        shiftBar = transform.GetChild(1).gameObject.GetComponent<Image>();
        borderBar = transform.GetChild(0).gameObject.GetComponent<Image>();
        UpdateColor();
    }

    void FixedUpdate()
    {
        if (disappear == true)
        {
            float a1 = 0f;
            float a2 = 0f;
            float a3 = 0f;

            if (stimulate == true)
            {
                a1 = Mathf.Lerp(baseBar.color.a, 1f, appearSpeed);
                a2 = Mathf.Lerp(shiftBar.color.a, 1f, appearSpeed);
                a3 = Mathf.Lerp(borderBar.color.a, 1f, appearSpeed);


                if (counter < disappearTime) { counter += Time.deltaTime; }
                else
                {
                    counter = 0f;
                    stimulate = false;
                }
            }
            else
            {
                a1 = Mathf.Lerp(baseBar.color.a, 0f, disappearSpeed);
                a2 = Mathf.Lerp(shiftBar.color.a, 0f, disappearSpeed);
                a3 = Mathf.Lerp(borderBar.color.a, 0f, disappearSpeed);
            }

            baseBar.color = new Color(baseBar.color.r, baseBar.color.g, baseBar.color.b, a1);
            shiftBar.color = new Color(shiftBar.color.r, shiftBar.color.g, shiftBar.color.b, a2);
            borderBar.color = new Color(borderBar.color.r, borderBar.color.g, borderBar.color.b, a3);
        }

        baseBar.fillAmount = health / maxHealth;
        shiftBar.fillAmount = Mathf.Lerp(shiftBar.fillAmount, baseBar.fillAmount, shiftSpeed);
    }

    public void Damage(float value, bool killingBlow = false)
    {
        Stimulate();

        float result = health - value;

        if (result > 0) { health = result; }
        else
        {
            if (killingBlow == true)
            {
                health = 0;

                EnemyBase owner = GetComponentInParent<EnemyBase>();
                if (owner != null) { owner.Death(); }
                else
                {
                    BossEnemy boss = GameObject.FindAnyObjectByType<BossEnemy>();
                    if(boss != null)
                    {
                        boss.Death();
                    }
                }
            }
            else
            {
                health = 1;
            }
        }

        baseBar.fillAmount = health / maxHealth;
        damage = true;
    }

    public void Stimulate()
    {
        if (disappear == true)
        {
            stimulate = true;
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
    }
}
