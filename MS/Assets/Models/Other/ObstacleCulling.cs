using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCulling : MonoBehaviour
{
    public List<Material> materials;
    public Camera camera;
    public LayerMask layer;
    public PlayerManager player;
    public float maskSize;

    public static int _PlayerPosition = Shader.PropertyToID("_PlayerPosition");
    public static int _MaskSize = Shader.PropertyToID("_MaskSize");

    private void Start()
    {
        player = FindAnyObjectByType<PlayerManager>();

        var renderers = GetComponentsInChildren<Renderer>();
        materials = new List<Material>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty(_PlayerPosition))
            {
                materials.Add(renderers[i].material);
            }
        }
    }
    void FixedUpdate()
    {
        var view = camera.WorldToViewportPoint(player.transform.position);

        for (int i = 0; i < materials.Count; i++)
        {
            var dir = camera.transform.position - player.transform.forward;
            //var ray = new Ray(player.transform.position, dir.normalized);

            //if(Physics.Raycast(ray, 3000, layer))
            //{
            //    materials[i].SetFloat(_Size, 1f);
            //}
            //else
            //{
            //    materials[i].SetFloat(_Size, 0f);
            //}

            materials[i].SetVector(_PlayerPosition, view);
            materials[i].SetFloat(_MaskSize, maskSize);
        }
    }
}
