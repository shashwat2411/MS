using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour
{
    public GameObject roomIconPrefab;       // 部屋アイコンのPrefab
    public GameObject TopBottomPrefab;
    public GameObject LeftRightPrefab;
    public RectTransform miniMapContainer; // ミニマップのUIコンテナ

    public RoomData[,] roomDatas;
    public float roomIconSizeY = 100f; // 部屋アイコンのサイズ
    public float roomIconSizeX = 100f; // 部屋アイコンのサイズ

    public float roomIconDirection = 0f; //部屋アイコンと部屋アイコンの距離

    //アイコンのPrefabs
    public GameObject PlayerIconPrefab;
    public GameObject StartIconPrefab;
    public GameObject ExitIconPrefab;


    private RoomManager roomManager;

    private int nowX, nowY;

    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        roomDatas = roomManager.GetRoomDatas();
        GenerateMiniMap(roomDatas);
        UpdateMiniMap();




    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) MovePlayer(0, -1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MovePlayer(0, 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MovePlayer(-1, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MovePlayer(1, 0);

    }

    void GenerateMiniMap(RoomData[,] datas )
    {
        for (int y = 0; y < datas.GetLength(0); y++)
        {
            for (int x = 0; x < datas.GetLength(1); x++)
            {
                int roomType = datas[y, x].roomNumber;
                if (roomType == 0) continue; // 部屋が存在しない場合はスキップ

                //部屋が存在する場合
                GameObject icon = Instantiate(roomIconPrefab, miniMapContainer);
                RectTransform rect = icon.GetComponent<RectTransform>();

                

                // アイコンの位置を設定
                rect.anchoredPosition = new Vector2(x * roomIconSizeX, -y * (rect.sizeDelta.y - 5) );

                //部屋の連結を調べる
                MakeCorridor(datas, x, y, rect);

                // 特別な部屋のアイコンに色を付ける 
                // 特定の位置にアイコンを作製する
                if (roomType == 1)
                {
                    icon.transform.GetChild(2).GetComponent<Image>().color = Color.green; // スタート地点
                    nowX = x;
                    nowY = y;

                    //// プレイヤーアイコン生成
                    //GameObject playerIcon = Instantiate(PlayerIconPrefab, miniMapContainer);
                    //RectTransform playerRect = playerIcon.GetComponent<RectTransform>();
                    //playerRect.anchoredPosition = new Vector2(nowX * roomIconSize, -nowY * roomIconSize);
                    //playerIcon.name = "PlayerIcon";
                }
                if (roomType == 99) icon.transform.GetChild(2).GetComponent<Image>().color = Color.red; // ゴール地点
            }
        }
    }

    void MakeCorridor(RoomData[,] roomDatas, int nx, int ny, RectTransform rf)
    {
        Debug.Log("壁を生成します");

        // 現在の部屋が存在しない場合は終了
        if (roomDatas[ny, nx].roomNumber <= 0) return;

        // 上方向の壁
        if (ny - 1 < 0 || roomDatas[ny - 1, nx].roomNumber <= 0)
        {
            CreateWall(TopBottomPrefab, rf, new Vector2(0, rf.sizeDelta.y / 2 - 2.5f), "上壁");
        }

        // 下方向の壁
        if (ny + 1 >= roomDatas.GetLength(0) || roomDatas[ny + 1, nx].roomNumber <= 0)
        {
            CreateWall(TopBottomPrefab, rf, new Vector2(0, -rf.sizeDelta.y / 2 + 2.5f), "下壁");
        }

        // 左方向の壁
        if (nx - 1 < 0 || roomDatas[ny, nx - 1].roomNumber <= 0)
        {
            CreateWall(LeftRightPrefab, rf, new Vector2(-rf.sizeDelta.x / 2 + 2.5f, 0), "左壁");
        }

        // 右方向の壁
        if (nx + 1 >= roomDatas.GetLength(1) || roomDatas[ny, nx + 1].roomNumber <= 0)
        {
            CreateWall(LeftRightPrefab, rf, new Vector2(rf.sizeDelta.x / 2 - 2.5f, 0), "右壁");
        }
    }

    // 壁を生成する汎用メソッド
    void CreateWall(GameObject prefab, RectTransform parent, Vector2 position, string direction)
    {
        Debug.Log(direction);
        GameObject wall = Instantiate(prefab, parent);
        RectTransform wallRect = wall.GetComponent<RectTransform>();
        wallRect.anchoredPosition = position;
    }


    //nowX,nowYに合わせて、miniMapContainerのRectTranceformを移動する。
    //nowX,nowYを0,0に持ってくる
    void UpdateMiniMap()
    {
        // 現在の部屋の位置を計算
        float offsetX = nowX * roomIconSizeX;
        float offsetY = -nowY * roomIconSizeY;

        // ミニマップコンテナの位置を調整
        miniMapContainer.anchoredPosition = new Vector2(-offsetX, -offsetY);
    }

    public void MovePlayer(int deltaX, int deltaY)
    {
        int newX = nowX + deltaX;
        int newY = nowY + deltaY;

        // 範囲外チェック
        if (newX < 0 || newX >= roomDatas.GetLength(1) || newY < 0 || newY >= roomDatas.GetLength(0))
        {
            Debug.Log("これ以上移動できません");
            return;
        }

        // 部屋が存在するか確認
        if (roomDatas[newY, newX].roomNumber == 0)
        {
            Debug.Log("存在しない部屋です");
            return;
        }

        // 現在位置を更新
        nowX = newX;
        nowY = newY;

        // プレイヤーアイコンの位置を更新
        //RectTransform playerIcon = miniMapContainer.Find("PlayerIcon").GetComponent<RectTransform>();
        //playerIcon.anchoredPosition = new Vector2(nowX * roomIconSize, -nowY * roomIconSize);


        // ミニマップを更新
        UpdateMiniMap();
    }

    public void DeleteMiniMap()
    {
        // ミニマップの全ての子オブジェクトを削除
        foreach (Transform child in miniMapContainer)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("ミニマップをリセットしました");
    }

    public void ResetAndGenerateMiniMap(RoomData[,] _datas)
    {
        // ミニマップをリセット
        DeleteMiniMap();

        // ミニマップを再生成
        GenerateMiniMap(_datas);

        Debug.Log("ミニマップを再生成しました");
    }

}
