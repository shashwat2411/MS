using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BulletBase : MonoBehaviour, IAtkEffBonusAdder
{
    public float damage;
    public float speed = 3f;
    public float lifetime = 10f;
    public float effectSize = 1f;

    bool once = true;
    float maxAttackSize;
    protected Vector3 hitPos = Vector3.zero;
    protected ChargePhase chargePhase;


    [SerializeField]
    GameObject impactEffect;

    [Header("SE")]
    public string nameSE;



    private PlayerManager player;

    protected static List<GameObject> sp = new List<GameObject>();
    protected static List<SpecialAttackFactory> spFactory = new List<SpecialAttackFactory>();


    public void Initiate(Vector3 direction, Vector3 hitPosition, float maxAttackSize = 100.0f, float damage = 1.0f, ChargePhase chargePhase = ChargePhase.Entry)
    {
        once = true;

        hitPos = hitPosition;

        this.damage = damage;
        this.chargePhase = chargePhase;
        this.maxAttackSize = maxAttackSize;

        GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        GetComponent<TrailRenderer>().time = 0.5f;

        Invoke("DestroyBullet", lifetime);

        player = FindFirstObjectByType<PlayerManager>();
    }


    void FixedUpdate()
    {

        if (transform.position.y < -0.8f)
        {
            CollisionProcess();
        }

        if (once)
        {
            transform.Rotate(Vector3.right * 500.0f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (once && other.CompareTag("Ground"))
        {
            Debug.Log(other.name);

            CollisionProcess();
        }
       
    }

    private void CollisionProcess()
    {
        //Position
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.transform.position = hitPos + Vector3.up * 0.05f;

        //Trail
        GetComponent<TrailRenderer>().time = 0f;

        //Scale
        GameObject effect = Instantiate(impactEffect, hitPos, transform.rotation);
        Vector3 localScale = new Vector3(effectSize, effectSize, effectSize);

        float scale = (this.damage / player.playerData.attack) / player.playerData.maxChargeTime;
        float finalScale = Mathf.Lerp(0.6f, 1.3f, scale);

        effect.transform.localScale = finalScale * localScale;
        effect.transform.position = hitPos + new Vector3(0f, -effect.transform.localScale.y / 2f, 0f);
        //if (scale <= this.maxAttackSize) {  }
        //else { effect.transform.localScale = this.maxAttackSize * localScale; }

        effect.GetComponent<MenkoExplosion>().damage = scale;
        effect.GetComponentInChildren<BulletImpact>().damage = this.damage;

        foreach(SpecialAttackFactory sp in spFactory)
        {
            if(sp.gameObject.GetComponent<GroundFireFactory>() != null)
            {
                effect.GetComponent<MenkoExplosion>().SwitchColors();
                break;
            }
        }
        //Sound Effect
        GameObject.FindAnyObjectByType<SoundsManager>().PlaySE(nameSE);

        //Invoke Stuff
        DoSpecialThings();
        //Invoke("DestroyBullet", lifetime);
        once = false;
        Destroy(gameObject);
    }

    void DestroyBullet()
    {
        CancelInvoke();

        ObjectPool.Instance.Push(gameObject);
    }

    public virtual void DoSpecialThings() { }


    public void ApplyBonus(GameObject bonusEffect)
    {
        sp.Add(bonusEffect);
        spFactory.Add(bonusEffect.GetComponent<SpecialAttackFactory>());
    }

    public void ResetBonus()
    {
        sp.Clear();
    }

}
