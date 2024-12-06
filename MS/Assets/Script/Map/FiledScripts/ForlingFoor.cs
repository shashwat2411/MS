using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===============================
//  
//  落下床用のスクリプト
//  
//===============================
public class ForlingFoor : MonoBehaviour
{
    public float fallDelay = 1f; //床が落ち恥じるまでの時間
    public float respawnDelay = 3f; //床が本野位置に戻るまでの時間
    public Material dissolveMaterial;
    public float dissolveSpeed = 1f;
    public GameEffects respawnEfect;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private float dissolveAmount = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (dissolveMaterial != null)
        {
            dissolveMaterial.SetFloat("_DissolveAmount", 0f); // 初期状態で表示
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーや特定のオブジェクトが接触したときに落下開始
        if (collision.gameObject.CompareTag("Player")) // "Player"タグが必要
        {
            Invoke("DropPlatform", fallDelay);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false; //落下を解放する
        Invoke("RespawnPlatform", respawnDelay);
    }

    private void RespawnPlatform()
    {
        Collider cl = GetComponent<Collider>();
        cl.enabled = false; //当たり判定を無効に
        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        //エフェクト作成
        if(respawnEfect != null)
        {

            Instantiate(respawnEfect,transform.position + Vector3.up, Quaternion.identity);
        }

        // ディゾルブで復活
        StartCoroutine(DissolveIn(cl));

    }

    private System.Collections.IEnumerator DissolveIn(Collider col)
    {
        dissolveAmount = 1f;
        if(dissolveMaterial != null)
        {
            while(dissolveAmount > 0f)
            {
                dissolveAmount -= Time.deltaTime * dissolveSpeed;
                dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
                yield return null;
            }
            dissolveMaterial.SetFloat("_DissolveAmount", 0f);
            col.enabled = true;
        }
    }
}
