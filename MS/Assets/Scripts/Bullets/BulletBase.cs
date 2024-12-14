using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BulletBase : MonoBehaviour, IAtkEffBonusAdder
{
    public float speed = 3f;
    public float lifetime = 10f;
    protected Vector3 hitPos = Vector3.zero;

    Collider collider;

    [SerializeField]
    GameObject impactArea;
    [SerializeField]
    GameObject impactEffect;

    public float damage;
    float maxAttackSize;

    protected ChargePhase chargePhase;

    float factor = 10.0f;


    bool once = true;

    protected static List<GameObject> sp = new List<GameObject>();
    protected static List<SpecialAttackFactory> spFactory = new List<SpecialAttackFactory>();


    public void Initiate(Vector3 direction, Vector3 hitPosition, float maxAttackSize = 100.0f, float damage = 1.0f, ChargePhase chargePhase = ChargePhase.Entry)
    {
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        once = true;
        this.damage = damage / factor;
        GetComponent<TrailRenderer>().time = 0.5f;

        this.chargePhase = chargePhase;
        hitPos = hitPosition;
        this.maxAttackSize = maxAttackSize;
        Invoke("DestroyBullet", lifetime);

    }


    private void OnTriggerEnter(Collider other)
    {

        if (once && other.CompareTag("Ground"))
        {
            Debug.Log(other.name);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //this.transform.position = new Vector3(this.transform.position.x, 0.2f, this.transform.position.z);
            this.transform.position = hitPos + Vector3.up * 0.05f;

            GetComponent<TrailRenderer>().time = 0f;
            impactArea.SetActive(true);
            impactEffect.SetActive(true);
            if (this.maxAttackSize >= this.damage / factor)
            {
                impactArea.transform.localScale = Vector3.one * this.damage / factor;
                impactEffect.transform.localScale = new Vector3(this.damage / factor, this.damage / factor, this.damage / (2 * factor));
            }
            else
            {
                impactArea.transform.localScale = Vector3.one * this.maxAttackSize;
                impactEffect.transform.localScale = new Vector3(this.maxAttackSize, this.maxAttackSize, this.maxAttackSize / 2);

            }

            // Debug.Log(this.totalDamage / factor);

            DoSpecialThings();
            Invoke("DestroyBullet", lifetime);
            once = false;
        }
    }

    void DestroyBullet()
    {
        CancelInvoke();
        impactArea.SetActive(false);
        impactEffect.SetActive(false);
        ObjectPool.Instance.Push(gameObject);


        //Destroy(impactArea);
        //Destroy(impactEffect);
        //Destroy(gameObject);
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
