using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : PlayerAbility
{
    public float rotationSpeed;
    protected override void Start()
    {
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position = player.transform.position;

        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        ThrowableEnemyObject thrownObject = other.GetComponent<ThrowableEnemyObject>();

        if (thrownObject != null)
        {
            thrownObject.SetTarget(thrownObject.GetOwner().transform.position);
            thrownObject.SetPlayer(thrownObject.GetOwner());
            thrownObject.SetOwner(player.gameObject);
            thrownObject.ResetMotion();
        }
    }
}
