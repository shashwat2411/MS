using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : ThrowableEnemyObject
{
    [HideInInspector] public bool start = false;
    private bool grounded = false;
    private float countdownValue;
    private Vector3 fixedPosition;

    public ParticleSystem[] explosionSystem;
    public MeshRenderer fuseMaterial;
    public string nameSE;

    public AnimationCurve expansion;
    private float expansionCounter = 0f;
    [HideInInspector] public Vector3 localScale;

    //Hash Map
    private int _Transparency = Shader.PropertyToID("_Transparency");
    protected override void Start()
    {
        base.Start();

        grounded = false;

        fuseMaterial.material = Instantiate(fuseMaterial.material);
    }

    protected override void FixedUpdate()
    {
        if (start == true)
        {
            base.FixedUpdate();

            if (grounded == true) { transform.position = fixedPosition; }

            countdownValue = (maxLifetime - lifetime) / maxLifetime;
            fuseMaterial.material.SetFloat(_Transparency, countdownValue);
        }
        else
        {
            if (expansionCounter < 1f) { expansionCounter += Time.deltaTime; }
            else { expansionCounter = 1f; }

            float size = expansion.Evaluate(expansionCounter);
            transform.localScale = localScale * size;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        SoundManager.Instance.PlaySE(nameSE);

        if (grounded == true)
        {
            transform.GetChild(0).parent = null;

            explosionSystem[0].gameObject.SetActive(true);
            explosionSystem[1].gameObject.SetActive(true);

            explosionSystem[0].Play();
            explosionSystem[1].Play();
        }
        else
        {
            explosionSystem[2].transform.parent = null;

            explosionSystem[2].gameObject.GetComponent<ParticleSystemDestroyer>().enabled = true;

            explosionSystem[2].gameObject.SetActive(true);
            explosionSystem[2].Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (other.gameObject == mainCharacter && owner != mainCharacter)
        //{
        //    PlayerManager manager = mainCharacter.GetComponent<PlayerManager>();
        //    //プレーヤーへのダメージ
        //    manager.playerHP.Damage(owner.GetComponent<EnemyBase>().attackPower);
        //    //other.GetComponent<MeshRenderer>().material.color = Color.green;
        //}
        if (collision.gameObject.GetComponent<ShieldAbility>() != null) { return; }

        if(collision.gameObject.CompareTag("Ground"))
        {
            fixedPosition = transform.position;
            grounded = true;
            return;
        }

        if (collision.gameObject == player && owner != player)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            EnemyBase enemyBase = player.GetComponent<EnemyBase>();

            if (playerManager != null) { playerManager.playerHP.Damage(damage); }
            else if (enemyBase != null) { enemyBase.Damage(damage); }

            Destroy(gameObject);
        }
        else if(owner == player)
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if(enemy != null)
            {
                enemy.Damage(damage);
                Destroy(gameObject);
            }
        }

        Destroy(gameObject);
    }
}