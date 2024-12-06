using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Position { get; set; }  // 部屋の座標
    public bool HasTopExit, HasBottomExit, HasLeftExit, HasRightExit;  // 各方向の接続情報
    public bool IsCleared = false;  // 敵を倒した後のフラグ
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
        Debug.Log("通路を生成します");
        //このスクリプトが挿入されているオブジェクトの子供からそれぞれを選ぶ
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
