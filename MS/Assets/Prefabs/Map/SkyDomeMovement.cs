using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDomeMovement : MonoBehaviour
{
    public Texture[] myTexture;
    private Material[] material;
    private Material mapMaterial;

    public float framerate = 1f / 30f;
    public float time = 0f;
    public int index = 0;
    public int maxIndex = 0;

    private bool once = false;
    public float transitionDuration;

    [ColorUsage(false, true)] public Color thresholdColor;
    [ColorUsage(false, true)] public Color mapThresholdColor;
    [ColorUsage(false, true)] private Color[] originalColor;
    [ColorUsage(false, true)] private Color originalMapColor;

    private int _Color = Shader.PropertyToID("_Color");
    private int _MainTex = Shader.PropertyToID("_MainTex");
    private int _EmissionColor = Shader.PropertyToID("_EmissionColor");

    public BossEnemy boss;
    private void Start()
    {
        once = false;

        time = 0f;
        index = 0;
        maxIndex = myTexture.Length - 1;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        material = new Material[renderers.Length];
        originalColor = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            material[i] = renderers[i].material;
        }

        GameObject map = GameObject.Find("BossMap(Clone)");
        if (map != null) { mapMaterial = map.transform.GetChild(0).GetComponent<MeshRenderer>().material; }
        else { mapMaterial = null; }

        for (int i = 0; i < material.Length; i++) { originalColor[i] = material[i].GetColor(_Color); }
        if (mapMaterial != null) { originalMapColor = mapMaterial.GetColor(_EmissionColor); }
    }
    private void FixedUpdate()
    {
        if(boss)
        {
            if(boss.bossHealthBar.health / boss.bossHealthBar.maxHealth < boss.phaseChangeThreshold && once == false)
            {
                once = true;
                StartCoroutine(ChangeColor(transitionDuration));
            }
        }
        else
        {
            for (int i = 0; i < material.Length; i++) { material[i].SetColor(_Color, originalColor[i]); }
            if (mapMaterial != null) { mapMaterial.SetColor(_EmissionColor, originalMapColor); }
        }

        if (time < framerate) { time += Time.deltaTime; }
        else 
        { 
            time -= framerate;
            if (index < maxIndex) { index++; }
            else { index -= maxIndex; }
        }


        for (int i = 0; i < material.Length; i++) { material[i].SetTexture(_MainTex, myTexture[index]); }
    }

    public IEnumerator ChangeColor(float duration)
    {
        float elapsed = 0f;

        while(elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            for (int i = 0; i < material.Length; i++) { material[i].SetColor(_Color, Color.Lerp(originalColor[i], thresholdColor, elapsed / duration)); }
            if (mapMaterial != null) { mapMaterial.SetColor(_EmissionColor, Color.Lerp(originalMapColor, mapThresholdColor, elapsed / duration)); }

            yield return null;
        }

        for (int i = 0; i < material.Length; i++) { material[i].SetColor(_Color, thresholdColor); }
        if (mapMaterial != null) { mapMaterial.SetColor(_EmissionColor, mapThresholdColor); }
    }
}
