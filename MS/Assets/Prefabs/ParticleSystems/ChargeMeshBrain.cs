using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMeshBrain : MonoBehaviour
{
    public AnimationCurve outline;
    public AnimationCurve damp;

    private float time;
    private Material material;
    private PlayerManager playerManager;
    void Start()
    {
        playerManager = FindFirstObjectByType<PlayerManager>();

        material = Instantiate(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = material;

        time = 0f;
    }

    void FixedUpdate()
    {
        time = (playerManager.playerData.charge - 1f) / (playerManager.playerData.maxChargeTime - 1f);


        Debug.Log(time);
        float x = outline.Evaluate(time);
        float y = damp.Evaluate(time);

        material.SetFloat("_OutlineScale", x);
        material.SetFloat("_Damp", y);
    }
}
