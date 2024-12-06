using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===============================
//  
//  �������p�̃X�N���v�g
//  
//===============================
public class ForlingFoor : MonoBehaviour
{
    public float fallDelay = 1f; //���������p����܂ł̎���
    public float respawnDelay = 3f; //�����{��ʒu�ɖ߂�܂ł̎���
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
            dissolveMaterial.SetFloat("_DissolveAmount", 0f); // ������Ԃŕ\��
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�����̃I�u�W�F�N�g���ڐG�����Ƃ��ɗ����J�n
        if (collision.gameObject.CompareTag("Player")) // "Player"�^�O���K�v
        {
            Invoke("DropPlatform", fallDelay);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false; //�������������
        Invoke("RespawnPlatform", respawnDelay);
    }

    private void RespawnPlatform()
    {
        Collider cl = GetComponent<Collider>();
        cl.enabled = false; //�����蔻��𖳌���
        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        //�G�t�F�N�g�쐬
        if(respawnEfect != null)
        {

            Instantiate(respawnEfect,transform.position + Vector3.up, Quaternion.identity);
        }

        // �f�B�]���u�ŕ���
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
