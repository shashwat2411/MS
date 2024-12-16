using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMeshBrain : MonoBehaviour
{
    public float alphaSpeed = 0.2f;
    public AnimationCurve outline;
    public AnimationCurve damp;

    private float time;
    private Color baseColor;
    private Color outlineColor;
    private Material material;
    private PlayerManager playerManager;
    private Animator animator;

    //Hash Map
    private int _OutlineScale = Shader.PropertyToID("_OutlineScale");
    private int _Damp = Shader.PropertyToID("_Damp");
    private int _BaseColor = Shader.PropertyToID("_BaseColor");
    private int _OutlineColor = Shader.PropertyToID("_OutlineColor");

    void Start()
    {
        playerManager = FindFirstObjectByType<PlayerManager>();

        material = Instantiate(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = material;

        time = 0f;

        baseColor = material.GetColor(_BaseColor);
        outlineColor = material.GetColor(_OutlineColor);

        animator = transform.parent.GetComponentInChildren<Animator>();

        PlayAnimation(false);
    }

    void FixedUpdate()
    {
        time = (playerManager.playerData.charge - 1f) / (playerManager.playerData.maxChargeTime - 1f);

        float x = outline.Evaluate(time);
        float y = damp.Evaluate(time);

        material.SetFloat(_OutlineScale, x);
        material.SetFloat(_Damp, y);

        float value = Mathf.Lerp(0f, 1f, time / alphaSpeed);
        material.SetColor(_BaseColor, new Color(baseColor.r, baseColor.g, baseColor.b, value));
        material.SetColor(_OutlineColor, new Color(outlineColor.r, outlineColor.g, outlineColor.b, value));
    }

    public void PlayAnimation(bool appear)
    {
        if (animator != null)
        {
            if (appear == true)
            {
                animator.Play("MenkoAppearingAnimation");
            }
            else
            {
                animator.Play("IdleMenko");
                
            }
        }
    }
}
