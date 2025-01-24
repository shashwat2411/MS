using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class Lighting : MonoBehaviour
{
    private int order = 0;
    //public int level = 1;
    public float totalDamage;
    public float offset = 3.0f;
    public float horizontalOffset = 2.0f;
    public AnimationCurve lightningCurve;

    [Header("SE")]
    public string nameSE;

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
        //Position
        PositionCalculator(usedMenko);


        //Damage Setting
        this.totalDamage = damage ;


        //VFX Setting
        lightningVfx = GetComponentInChildren<VisualEffect>();

        color1[0] = lightningVfx.GetVector4(_LightningColor);
        color1[1] = lightningVfx.GetVector4(_Color);

        if (index == 0 || index == 1)
        {
            lightningVfx.SetVector4(_LightningColor, color1[0]);
            lightningVfx.SetVector4(_Color, color1[1]);
        }
        else if (index == 2 || index == 3)
        {
            lightningVfx.SetVector4(_LightningColor, color2[0]);
            lightningVfx.SetVector4(_Color, color2[1]);
        }
        else
        {
            lightningVfx.SetVector4(_LightningColor, color3[0]);
            lightningVfx.SetVector4(_Color, color3[1]);
        }

       
    }

    private void Awake()
    {
        //Sound Effect
        SoundManager.Instance.PlaySE(nameSE);
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(totalDamage);
            Debug.Log(enemy.name + totalDamage);
            
        }
        else // Boss
        {
            
            enemy = other.gameObject.GetComponentInParent<EnemyBase>();
            if (enemy)
            {
                enemy.Damage(totalDamage);
                Debug.Log(enemy.name + "  " + totalDamage);
                
            }
        }


       

    }

    public void SetOrder(int value) { order = value; }

    private void PositionCalculator(Transform usedMenko)
    {
        GameObject player = FindFirstObjectByType<PlayerManager>().gameObject;
        Vector3 direction = (usedMenko.transform.position - player.transform.position).normalized;
        direction.y = 0f;

        Vector3 position = usedMenko.position + direction * offset;

        switch (order)
        {
            case 0:
                {
                    transform.position = position;
                    break;
                }
            case 1:
                {
                    Vector3 sideDirection = Vector3.Cross(Vector3.up, direction);
                    transform.position = position + sideDirection * horizontalOffset;
                    break;
                }
            case 2:
                {
                    Vector3 sideDirection = Vector3.Cross(direction, Vector3.up);
                    transform.position = position + sideDirection * horizontalOffset;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
