using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //==================================
    // ルーム情報取得
    //==================================
    public RoomInfo roomInfo;

    //==================================
    // CSVファイル読み込みなど
    //==================================
    public string[] csvFiles;

    #region ルームデータ
    public RoomData[,] roomDatas;
    public GameObject currentRoom; // 現在の部屋の参照
    public int nowH;  //縦
    public int nowW;  //横

    #endregion

    #region デバッグ用
    public bool Iscleared;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        
        CSVReading(0);
        currentRoom = MakeRoom(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveRoom(-1, 0);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveRoom(1, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveRoom(0, -1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRoom(0, 1);
    }

    //==================================
    // CSVファイル読み込み
    //==================================
    void CSVReading(int num)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvFiles[num]);
        if (csvFile == null)
        {
            Debug.LogError(csvFiles[num] + "の名前のCSVファイルが見つかりません");
            return;
        }

        string[] rows = csvFile.text.Split('\n');
        int rowCount = rows.Length;

        // 列数を最大値で決定
        int colCount = 0;
        foreach (var row in rows)
        {
            int colsLength = row.Split(',').Length;
            if (colsLength > colCount)
                colCount = colsLength;
        }

        // 配列を初期化
        roomDatas = new RoomData[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] cols = rows[i].Split(',');

            for (int j = 0; j < cols.Length; j++) // 実際の列数を確認
            {
                if (int.TryParse(cols[j], out int value))
                {
                    // 部屋番号をセットし、初期状態は未クリアとする
                    roomDatas[i, j] = new RoomData(value, false, false);

                    // スタート地点（部屋番号1）を記録
                    if (value == 1)
                    {
                        nowH = i;
                        nowW = j;
                    }

                    print(value);
                }
                else
                {
                    // 無効データの場合
                    roomDatas[i, j] = new RoomData(0, false, false);
                }
            }

            // 足りない列を補完
            for (int j = cols.Length; j < colCount; j++)
            {
                roomDatas[i, j] = new RoomData(0, false, false);
            }
        }

        Debug.Log("CSVデータを正常に読み込みました");
    }

    //==================================
    // 部屋を移動
    //==================================
    void MoveRoom(int deltaH, int deltaW)
    {
        int newH = nowH + deltaH;
        int newW = nowW + deltaW;

        // 範囲外チェック
        if (newH < 0 || newH >= roomDatas.GetLength(0) || newW < 0 || newW >= roomDatas.GetLength(1))
        {
            Debug.Log("これ以上移動できません");
            return;
        }

        RoomData nextRoom = roomDatas[newH, newW];

        if (nextRoom.roomNumber <= 0)
        {
            Debug.Log("存在しない部屋です");
            return;
        }

        // 現在の部屋を削除
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        roomDatas[nowH,nowW].isCleared = true;

        // クリア済みか確認
        if (nextRoom.isCleared)
        {
            Debug.Log("この部屋はクリア済みです");
        }
        else
        {
            Debug.Log("新しい部屋をロードします");
            roomDatas[newH, newW] = nextRoom; // 状態を更新
        }

        // 現在の位置を更新
        nowH = newH;
        nowW = newW;

        // 部屋を生成
        currentRoom = MakeRoom(nextRoom.roomNumber);
    }

    //==================================
    // ルーム制作
    //==================================

    GameObject MakeRoom(int no)
    {
        //==================================
        // ラムダ式 第二引数はPredicate
        // RoomCoreクラスの配列内に
        // 同じ名前の物があるかどうかを確認
        //==================================
        RoomCore r = System.Array.Find(roomInfo.GetRooms(), room => room.no == no);

        //あるかどうかチェック
        if (r == null)
        {
            print(no + "の番号を持つ部屋が見つかりませんでした");
            print("入力した番号を再確認してください");
            return null;
        }

        // 部屋を作製
        GameObject room = Instantiate(r.room, new Vector3(0, 0, 0), Quaternion.identity);
        //上下左右に部屋があるかどうか確認

        Iscleared = roomDatas[nowH, nowW].isCleared;

        // 敵の生成制御
        if (!roomDatas[nowH, nowW].isCleared)
        {
            Debug.Log("敵を生成します");
            // 敵生成ロジックを追加
        }
        else
        {
            Debug.Log("クリア済みのため敵は生成されません");
        }

        return room;
    }

    //=======================
    // 外部参照用
    //=======================
    public RoomData[,] GetRoomDatas() { return roomDatas; }
}


