using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int rows = 3;  // マップの行数
    public int cols = 5;  // マップの列数
    public GameObject[] roomPrefabs; // 部屋の地形Prefabのバリエーション
    public GameObject entrancePrefab; // 入口用Prefab
    public GameObject exitPrefab;     // 出口用Prefab

    private Room[,] rooms;

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        //rooms = new Room[cols, rows];
        //Transform mapHolder = new GameObject("Map").transform;

        //// 各部屋を生成
        //for (int x = 0; x < cols; x++)
        //{
        //    for (int y = 0; y < rows; y++)
        //    {
        //        Vector3 position = new Vector3(x * 10, 0, y * 10);

        //        // ランダムに地形Prefabを選択して配置
        //        GameObject selectedPrefab;
        //        if (x == 0 && y == 0)
        //        {
        //            // 入口の部屋
        //            selectedPrefab = entrancePrefab;
        //        }
        //        else if (x == cols - 1 && y == rows - 1)
        //        {
        //            // 出口の部屋
        //            selectedPrefab = exitPrefab;
        //        }
        //        else
        //        {
        //            // ランダムに部屋の地形を選択
        //            selectedPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        //        }

        //        GameObject roomInstance = Instantiate(selectedPrefab, position, Quaternion.identity);
        //        roomInstance.transform.SetParent(mapHolder);

        //        // Roomコンポーネントを取得して、位置情報を設定
        //        Room room = roomInstance.GetComponent<Room>();
        //        room.Position = new Vector2Int(x, y);
        //        rooms[x, y] = room;
        //    }
        //}

        //// 部屋同士の接続を確保
        //ConnectRooms();


        // 通常のDFSベースの部屋接続生成
        rooms = new Room[cols, rows];
        Transform mapHolder = new GameObject("Map").transform;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x * 10, 0, y * 10); // 修正ポイント
                GameObject selectedPrefab = (x == 0 && y == 0) ? entrancePrefab : (x == cols - 1 && y == rows - 1) ? exitPrefab : roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                GameObject roomInstance = Instantiate(selectedPrefab, position, Quaternion.identity);
                roomInstance.transform.SetParent(mapHolder);

                Room room = roomInstance.GetComponent<Room>();
                room.Position = new Vector2Int(x, y);
                rooms[x, y] = room;
            }
        }

        ConnectRooms(); // 通常の接続
        AddRandomConnections(3); // 追加接続で分岐を増やす
    }

    void ConnectRooms()
    {
        Stack<Room> stack = new Stack<Room>();
        Room currentRoom = rooms[0, 0];
        HashSet<Room> visitedRooms = new HashSet<Room> { currentRoom };

        while (visitedRooms.Count < cols * rows)
        {
            List<Room> neighbors = GetUnvisitedNeighbors(currentRoom, visitedRooms);

            if (neighbors.Count > 0)
            {
                Room nextRoom = neighbors[Random.Range(0, neighbors.Count)];
                ConnectTwoRooms(currentRoom, nextRoom);
                stack.Push(currentRoom);
                currentRoom = nextRoom;
                visitedRooms.Add(currentRoom);
            }
            else if (stack.Count > 0)
            {
                currentRoom = stack.Pop();
            }
        }
    }

    List<Room> GetUnvisitedNeighbors(Room room, HashSet<Room> visitedRooms)
    {
        List<Room> neighbors = new List<Room>();
        Vector2Int pos = room.Position;

        if (pos.x > 0 && !visitedRooms.Contains(rooms[pos.x - 1, pos.y])) neighbors.Add(rooms[pos.x - 1, pos.y]);
        if (pos.x < cols - 1 && !visitedRooms.Contains(rooms[pos.x + 1, pos.y])) neighbors.Add(rooms[pos.x + 1, pos.y]);
        if (pos.y > 0 && !visitedRooms.Contains(rooms[pos.x, pos.y - 1])) neighbors.Add(rooms[pos.x, pos.y - 1]);
        if (pos.y < rows - 1 && !visitedRooms.Contains(rooms[pos.x, pos.y + 1])) neighbors.Add(rooms[pos.x, pos.y + 1]);

        return neighbors;
    }

    void ConnectTwoRooms(Room room1, Room room2)
    {
        Vector2Int pos1 = room1.Position;
        Vector2Int pos2 = room2.Position;

        if (pos1.x == pos2.x)
        {
            if (pos1.y < pos2.y)
            {
                room1.HasTopExit = true;
                room2.HasBottomExit = true;
            }
            else
            {
                room1.HasBottomExit = true;
                room2.HasTopExit = true;
            }
        }
        else if (pos1.y == pos2.y)
        {
            if (pos1.x < pos2.x)
            {
                room1.HasRightExit = true;
                room2.HasLeftExit = true;
            }
            else
            {
                room1.HasLeftExit = true;
                room2.HasRightExit = true;
            }
        }

        // 各部屋の接続情報に基づき、通路設定を更新
        room1.MakingCorridor();
        room2.MakingCorridor();
    }

    void AddRandomConnections(int extraConnections)
    {
        int addedConnections = 0;

        while (addedConnections < extraConnections)
        {
            int x = Random.Range(0, cols);
            int y = Random.Range(0, rows);

            Room room = rooms[x, y];
            List<Room> possibleConnections = new List<Room>();

            if (x > 0 && !room.HasLeftExit) possibleConnections.Add(rooms[x - 1, y]);
            if (x < cols - 1 && !room.HasRightExit) possibleConnections.Add(rooms[x + 1, y]);
            if (y > 0 && !room.HasBottomExit) possibleConnections.Add(rooms[x, y - 1]);
            if (y < rows - 1 && !room.HasTopExit) possibleConnections.Add(rooms[x, y + 1]);

            if (possibleConnections.Count > 0)
            {
                Room randomNeighbor = possibleConnections[Random.Range(0, possibleConnections.Count)];
                ConnectTwoRooms(room, randomNeighbor);
                addedConnections++;
            }
        }
    }

}
