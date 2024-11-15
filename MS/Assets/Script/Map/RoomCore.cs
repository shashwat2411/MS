using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomCore
{
    [Tooltip("ルームの名称")]
    public string name;

    [Tooltip("番号")]
    public int no;

    [Tooltip("prefab")]
    public GameObject room;


    //通路を設置する場所
    public bool topCorridor = false;
    public bool bottomCorridor = false;
    public bool leftCorridor = false;
    public bool rightCorridor = false;

    //エネミーが討伐されてるか
    //public bool isCleared = false;
}
