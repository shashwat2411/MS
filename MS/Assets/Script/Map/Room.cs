using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Position { get; set; }  // �����̍��W
    public bool HasTopExit, HasBottomExit, HasLeftExit, HasRightExit;  // �e�����̐ڑ����
    public bool IsCleared = false;  // �G��|������̃t���O
    public GameObject topCorridor, bottomCorridor, leftCorridor, rightCorridor;

    private void Start()
    {
        MakingCorridor();
    }

    private void Update()
    {
        
    }

    public void MakingCorridor()
    {
        Debug.Log("�ʘH�𐶐����܂�");
        //���̃X�N���v�g���}������Ă���I�u�W�F�N�g�̎q�����炻�ꂼ���I��
        if (HasTopExit)
        {
            //GameObject topCorridor = transform.Find("corridor_parent/Top_Corridor").gameObject;
            topCorridor.SetActive(true);
        }
        if(HasBottomExit)
        {
            //GameObject bottomCorridor = transform.Find("corridor_parent/Bottom_Corridor").gameObject;
            bottomCorridor.SetActive(true);
        }
        if (HasLeftExit)
        {
            //GameObject leftCorridor = transform.Find("corridor_parent/Left_Corridor").gameObject;
            leftCorridor.SetActive(true);
        }
        if (HasRightExit)
        {
            //GameObject rightCorridor = transform.Find("corridor_parent/Right_Corridor").gameObject;
            rightCorridor.SetActive(true);
        }
    }
}
