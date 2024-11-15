using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomCore
{
    [Tooltip("���[���̖���")]
    public string name;

    [Tooltip("�ԍ�")]
    public int no;

    [Tooltip("prefab")]
    public GameObject room;


    //�ʘH��ݒu����ꏊ
    public bool topCorridor = false;
    public bool bottomCorridor = false;
    public bool leftCorridor = false;
    public bool rightCorridor = false;

    //�G�l�~�[����������Ă邩
    //public bool isCleared = false;
}
