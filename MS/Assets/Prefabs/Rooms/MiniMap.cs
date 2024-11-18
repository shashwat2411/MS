using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour
{
    public GameObject roomIconPrefab;       // 部屋アイコンのPrefab
    public RectTransform miniMapContainer; // ミニマップのUIコンテナ

    public RoomData[,] roomDatas;
    public float roomIconSize = 50f; // 部屋アイコンのサイズ

    void Start()
    {
        RoomManager roomManager = GetComponent<RoomManager>();
        roomDatas = roomManager.GetRoomDatas();
        GenerateMiniMap();

    }

    void GenerateMiniMap()
    {
        for (int y = 0; y < roomDatas.GetLength(0); y++)
        {
            for (int x = 0; x < roomDatas.GetLength(1); x++)
            {
                int roomType = roomDatas[y, x].roomNumber;
                if (roomType == 0) continue; // 部屋が存在しない場合はスキップ

                //部屋が存在する場合
                GameObject icon = Instantiate(roomIconPrefab, miniMapContainer);
                RectTransform rect = icon.GetComponent<RectTransform>();

                // アイコンの位置を設定
                rect.anchoredPosition = new Vector2(x * roomIconSize, -y * roomIconSize);

                // 特別な部屋のアイコンに色を付ける 
                // 特定の位置にアイコンを作製する
                if (roomType == 1) icon.GetComponent<Image>().color = Color.green; // スタート地点
                if (roomType == 99) icon.GetComponent<Image>().color = Color.red; // ゴール地点
            }
        }
    }
}
