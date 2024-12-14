using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class Lighting : MonoBehaviour
{
    public int level = 1;
    public float totalDamage;
    public float offset = 3.0f;
    public AnimationCurve lightningCurve;

    [Header("Damage Level Color")]
    [ColorUsage(false, true)] private Color[] color1 = new Color[2];
    [ColorUsage(false, true)] public Color[] color2 = new Color[2];
    [ColorUsage(false, true)] public Color[] color3 = new Color[2];

    //Hash Map
    private int _LightningColor = Shader.PropertyToID("LightningColor");
    private int _Color = Shader.PropertyToID("Color");




    private VisualEffect lightningVfx;
  

    public void Initiate(float lifetime = 0.8f, float damage = 1.0f, int index = 0, int levelDataCount = 0,Transform usedMenko = null)
    {
        //Random Position
        GameObject player = FindFirstObjectByType<PlayerManager>().gameObject;
        Vector3 direction = (usedMenko.transform.position - player.transform.position).normalized;
        transform.position = usedMenko.position + direction * offset;


        //Damage Setting
        this.totalDamage = damage ;


        //VFX Setting
        lightningVfx = GetComponentInChildren<VisualEffect>();

        color1[0] = lightningVfx.GetVector4(_LightningColor);
        color1[1] = lightningVfx.GetVector4(_Color);

        if (index == 0)
        {
            lightningVfx.SetVector4(_LightningColor, color1[0]);
            lightningVfx.SetVector4(_Color, color1[1]);
        }
        else if (index == (levelDataCount / 2))
        {
            lightningVfx.SetVector4(_LightningColor, color2[0]);
            lightningVfx.SetVector4(_Color, color2[1]);
        }
        else if (index == (levelDataCount - 1))
        {
            lightningVfx.SetVector4(_LightningColor, color2[0]);
            lightningVfx.SetVector4(_Color, color2[1]);
        }

       
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(totalDamage);
        }
    }
}
