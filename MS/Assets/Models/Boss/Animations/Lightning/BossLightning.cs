using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BossLightning : MonoBehaviour
{
    [HideInInspector] public float damage;
    private VisualEffect lightning;
    private void Awake()
    {
        lightning = GetComponentInChildren<VisualEffect>();
        lightning.Play();
    }

    private void FixedUpdate()
    {
        if (lightning.culled) { Debug.Log("Culled"); }

        if (lightning.HasAnySystemAwake() == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if(player)
        {
            player.playerHP.Damage(damage);
        }
    }
}
