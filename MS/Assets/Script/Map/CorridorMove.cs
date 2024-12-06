using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CorridorMove : MonoBehaviour
{
    // �ʘH�̂Ƃ���ɃC���|�[�g����
    // �����𖞂����Έړ����邱�Ƃ��ł���
    RoomManager roomManager;
    MiniMapUI miniMap;

    public int deltaH; // �����ړ��p�̏㉺����
    public int deltaW; // �����ړ��p�̍��E����


    private void Start()
    {
        //�Q�[������RoomManager���Z�b�g����Ă�����T��
        roomManager = FindObjectOfType<RoomManager>();
        miniMap = FindObjectOfType<MiniMapUI>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                roomManager.MoveRoom(deltaH, deltaW); // �������ړ�
                                                      //�v���C���[�̈ʒu��ς���
                miniMap.MovePlayer(deltaW, deltaH);
            }
        }
    }


}
