using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Position { get; private set; }  // 部屋の座標
    public bool HasTopExit, HasBottomExit, HasLeftExit, HasRightExit;  // 各方向の接続情報
    public bool IsCleared = false;  // 敵を倒した後のフラグ

    private void Start()
    {
        MakingCorridor();
    }

    private void Update()
    {
        MakingCorridor();  
    }

    public void MakingCorridor()
    {
        //このスクリプトが挿入されているオブジェクトの子供からそれぞれを選ぶ
        if (HasTopExit)
        {
            GameObject topCorridor = transform.Find("corridor_parent/Top_Corridor").gameObject;
            topCorridor.SetActive(false);
        }
        if(HasBottomExit)
        {
            GameObject bottomCorridor = transform.Find("corridor_parent/Bottom_Corridor").gameObject;
            bottomCorridor.SetActive(false);
        }
        if (HasLeftExit)
        {
            GameObject leftCorridor = transform.Find("corridor_parent/Left_Corridor").gameObject;
            leftCorridor.SetActive(false);
        }
        if (HasRightExit)
        {
            GameObject rightCorridor = transform.Find("corridor_parent/Right_Corridor").gameObject;
            rightCorridor.SetActive(false);
        }
    }
}
