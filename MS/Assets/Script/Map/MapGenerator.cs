using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int rows = 3;  // �}�b�v�̍s��
    public int cols = 5;  // �}�b�v�̗�
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
        //rooms = new Room[cols, rows];
        //Transform mapHolder = new GameObject("Map").transform;

        //// �e�����𐶐�
        //for (int x = 0; x < cols; x++)
        //{
        //    for (int y = 0; y < rows; y++)
        //    {
        //        Vector3 position = new Vector3(x * 10, 0, y * 10);

        //        // �����_���ɒn�`Prefab��I�����Ĕz�u
        //        GameObject selectedPrefab;
        //        if (x == 0 && y == 0)
        //        {
        //            // �����̕���
        //            selectedPrefab = entrancePrefab;
        //        }
        //        else if (x == cols - 1 && y == rows - 1)
        //        {
        //            // �o���̕���
        //            selectedPrefab = exitPrefab;
        //        }
        //        else
        //        {
        //            // �����_���ɕ����̒n�`��I��
        //            selectedPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        //        }

        //        GameObject roomInstance = Instantiate(selectedPrefab, position, Quaternion.identity);
        //        roomInstance.transform.SetParent(mapHolder);

        //        // Room�R���|�[�l���g���擾���āA�ʒu����ݒ�
        //        Room room = roomInstance.GetComponent<Room>();
        //        room.Position = new Vector2Int(x, y);
        //        rooms[x, y] = room;
        //    }
        //}

        //// �������m�̐ڑ����m��
        //ConnectRooms();


        // �ʏ��DFS�x�[�X�̕����ڑ�����
        rooms = new Room[cols, rows];
        Transform mapHolder = new GameObject("Map").transform;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x * 10, 0, y * 10); // �C���|�C���g
                GameObject selectedPrefab = (x == 0 && y == 0) ? entrancePrefab : (x == cols - 1 && y == rows - 1) ? exitPrefab : roomPrefabs[Random.Range(0, roomPrefabs.Length)];
                GameObject roomInstance = Instantiate(selectedPrefab, position, Quaternion.identity);
                roomInstance.transform.SetParent(mapHolder);

                Room room = roomInstance.GetComponent<Room>();
                room.Position = new Vector2Int(x, y);
                rooms[x, y] = room;
            }
        }

        ConnectRooms(); // �ʏ�̐ڑ�
        AddRandomConnections(3); // �ǉ��ڑ��ŕ���𑝂₷
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

        // �e�����̐ڑ����Ɋ�Â��A�ʘH�ݒ���X�V
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
