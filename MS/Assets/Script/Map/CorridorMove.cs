using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CorridorMove : MonoBehaviour
{
    // 通路のところにインポートする
    // 条件を満たせば移動することができる
    RoomManager roomManager;
    MiniMapUI miniMap;

    public int deltaH; // 部屋移動用の上下方向
    public int deltaW; // 部屋移動用の左右方向


    private void Start()
    {
        //ゲーム内のRoomManagerがセットされているやつを探す
        roomManager = FindObjectOfType<RoomManager>();
        miniMap = FindObjectOfType<MiniMapUI>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                roomManager.MoveRoom(deltaH, deltaW); // 部屋を移動
                                                      //プレイヤーの位置を変える
                miniMap.MovePlayer(deltaW, deltaH);
            }
        }
    }


}
