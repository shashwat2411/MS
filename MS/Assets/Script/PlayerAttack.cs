using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Close Range")]
    public float attackTime = 0.1f;
    public float damage = 10f;
    public GameObject collider;

    [Header("Long Range")]
    public float cooldown = 0.1f;
    public GameObject bullet;
    private bool shoot = true;

    [Header("InputSystem")]
    public InputActionReference closeRangeAttack;
    public InputActionReference longRangeAttack;

    void Start()
    {
        closeRangeAttack.action.started += CloseRangeAttack;
        longRangeAttack.action.started += LongRangeAttack;

        ResetCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void CloseRangeAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Close Range Attack");
        Invoke("ResetCollider", attackTime);

        collider.GetComponent<MeshRenderer>().enabled = true;
        collider.GetComponent<BoxCollider>().enabled = true;
    }
    void ResetCollider()
    {
        collider.GetComponent<MeshRenderer>().enabled = false;
        collider.GetComponent<BoxCollider>().enabled = false;
    }

    void LongRangeAttack(InputAction.CallbackContext context)
    {
        if (shoot == true)
        {
            Debug.Log("Long Range Attack");
            Instantiate(bullet, collider.transform.position, collider.transform.rotation).GetComponent<Bullet>().Initiate(transform.forward);
            Invoke("ResetCooldown", cooldown);
            shoot = false;
        }
    }

    void ResetCooldown()
    {
        shoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.Damage(damage);
            Debug.Log(damage);
        }
    }
}
