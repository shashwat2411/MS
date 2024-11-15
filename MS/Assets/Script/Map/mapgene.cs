using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapgene : MonoBehaviour
{
    public int rows = 3;
    public int cols = 5;
    public GameObject[] roomPrefabs; // �����̒n�`Prefab�̃o���G�[�V����
    public GameObject entrancePrefab; // �����pPrefab
    public GameObject exitPrefab;     // �o���pPrefab

    private Room[,] rooms;

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        rooms = new Room[cols, rows];
        Transform mapHolder = new GameObject("Map").transform;

        // �e�����𐶐�
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x * 10, 0, y * 10); // y���W���������ɍL����
                GameObject selectedPrefab;

                if (x == 0 && y == 0)
                {
                    selectedPrefab = entrancePrefab; // �����̕���
                }
                else if (x == cols - 1 && y == rows - 1)
                {
                    selectedPrefab = exitPrefab; // �o���̕���
                }
                else
                {
                    selectedPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)]; // �����_���n�`�̕���
                }

                GameObject roomInstance = Instantiate(selectedPrefab, position, Quaternion.identity);
                roomInstance.transform.SetParent(mapHolder);

                Room room = roomInstance.GetComponent<Room>();
                room.Position = new Vector2Int(x, y);
                rooms[x, y] = room;
            }
        }

        // �������m�̐ڑ����m��
        ConnectRooms();

        // �S�[���̕����͗B��̐ڑ��ɐ���
        SetSingleConnectionToExit();

        // �����_���Ƀf�b�h�G���h��ǉ�
        AddDeadEnds(3);
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

    void SetSingleConnectionToExit()
    {
        Room exitRoom = rooms[cols - 1, rows - 1];

        // ���݂̑S�Ă̐ڑ�������
        DisconnectAllNeighbors(exitRoom);

        // �o�������̗אڕ������烉���_����1��I�����A�B��̐ڑ��Ƃ���
        List<Room> neighbors = GetVisitedNeighbors(exitRoom);
        if (neighbors.Count > 0)
        {
            Room connectedRoom = neighbors[Random.Range(0, neighbors.Count)];
            ConnectTwoRooms(exitRoom, connectedRoom); // 1�̕��������ɐڑ�
        }
    }


    List<Room> GetVisitedNeighbors(Room room)
    {
        List<Room> neighbors = new List<Room>();
        Vector2Int pos = room.Position;

        if (pos.x > 0 && rooms[pos.x - 1, pos.y] != null) neighbors.Add(rooms[pos.x - 1, pos.y]);
        if (pos.x < cols - 1 && rooms[pos.x + 1, pos.y] != null) neighbors.Add(rooms[pos.x + 1, pos.y]);
        if (pos.y > 0 && rooms[pos.x, pos.y - 1] != null) neighbors.Add(rooms[pos.x, pos.y - 1]);
        if (pos.y < rows - 1 && rooms[pos.x, pos.y + 1] != null) neighbors.Add(rooms[pos.x, pos.y + 1]);

        return neighbors;
    }

    void DisconnectAllNeighbors(Room room)
    {
        room.HasTopExit = room.HasBottomExit = room.HasLeftExit = room.HasRightExit = false;
        room.MakingCorridor();
    }

    void AddDeadEnds(int count)
    {
        int added = 0;
        while (added < count)
        {
            int x = Random.Range(0, cols);
            int y = Random.Range(0, rows);

            Room room = rooms[x, y];
            if (room == null || room == rooms[0, 0] || room == rooms[cols - 1, rows - 1]) continue;

            List<Room> neighbors = GetVisitedNeighbors(room);
            if (neighbors.Count > 1)
            {
                // �����_����1�̗אڕ����Ƃ����ڑ����c��
                Room connectedRoom = neighbors[Random.Range(0, neighbors.Count)];
                DisconnectAllNeighbors(room);
                ConnectTwoRooms(room, connectedRoom);
                added++;
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

        room1.MakingCorridor();
        room2.MakingCorridor();
    }
}
