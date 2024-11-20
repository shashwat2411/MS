using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public int roomNumber; // �����ԍ�
    public bool isCleared; // �N���A���
    public bool isVisitor; // �K�ꂽ���ǂ���

    public RoomData(int number, bool cleared, bool visitored)
    {
        roomNumber = number;
        isCleared = cleared;
        isVisitor = visitored;
    }
}