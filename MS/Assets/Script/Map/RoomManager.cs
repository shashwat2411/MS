using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //==================================
    // ���[�����擾
    //==================================
    public RoomInfo roomInfo;

    //==================================
    // CSV�t�@�C���ǂݍ��݂Ȃ�
    //==================================
    public string[] csvFiles;

    #region ���[���f�[�^
    public RoomData[,] roomDatas;
    public GameObject currentRoom; // ���݂̕����̎Q��
    public int nowH;  //�c
    public int nowW;  //��

    #endregion

    #region �f�o�b�O�p
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
    // CSV�t�@�C���ǂݍ���
    //==================================
    void CSVReading(int num)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(csvFiles[num]);
        if (csvFile == null)
        {
            Debug.LogError(csvFiles[num] + "�̖��O��CSV�t�@�C����������܂���");
            return;
        }

        string[] rows = csvFile.text.Split('\n');
        int rowCount = rows.Length;

        // �񐔂��ő�l�Ō���
        int colCount = 0;
        foreach (var row in rows)
        {
            int colsLength = row.Split(',').Length;
            if (colsLength > colCount)
                colCount = colsLength;
        }

        // �z���������
        roomDatas = new RoomData[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] cols = rows[i].Split(',');

            for (int j = 0; j < cols.Length; j++) // ���ۂ̗񐔂��m�F
            {
                if (int.TryParse(cols[j], out int value))
                {
                    // �����ԍ����Z�b�g���A������Ԃ͖��N���A�Ƃ���
                    roomDatas[i, j] = new RoomData(value, false, false);

                    // �X�^�[�g�n�_�i�����ԍ�1�j���L�^
                    if (value == 1)
                    {
                        nowH = i;
                        nowW = j;
                    }

                    print(value);
                }
                else
                {
                    // �����f�[�^�̏ꍇ
                    roomDatas[i, j] = new RoomData(0, false, false);
                }
            }

            // ����Ȃ����⊮
            for (int j = cols.Length; j < colCount; j++)
            {
                roomDatas[i, j] = new RoomData(0, false, false);
            }
        }

        Debug.Log("CSV�f�[�^�𐳏�ɓǂݍ��݂܂���");
    }

    //==================================
    // �������ړ�
    //==================================
    void MoveRoom(int deltaH, int deltaW)
    {
        int newH = nowH + deltaH;
        int newW = nowW + deltaW;

        // �͈͊O�`�F�b�N
        if (newH < 0 || newH >= roomDatas.GetLength(0) || newW < 0 || newW >= roomDatas.GetLength(1))
        {
            Debug.Log("����ȏ�ړ��ł��܂���");
            return;
        }

        RoomData nextRoom = roomDatas[newH, newW];

        if (nextRoom.roomNumber <= 0)
        {
            Debug.Log("���݂��Ȃ������ł�");
            return;
        }

        // ���݂̕������폜
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        roomDatas[nowH,nowW].isCleared = true;

        // �N���A�ς݂��m�F
        if (nextRoom.isCleared)
        {
            Debug.Log("���̕����̓N���A�ς݂ł�");
        }
        else
        {
            Debug.Log("�V�������������[�h���܂�");
            roomDatas[newH, newW] = nextRoom; // ��Ԃ��X�V
        }

        // ���݂̈ʒu���X�V
        nowH = newH;
        nowW = newW;

        // �����𐶐�
        currentRoom = MakeRoom(nextRoom.roomNumber);
    }

    //==================================
    // ���[������
    //==================================

    GameObject MakeRoom(int no)
    {
        //==================================
        // �����_�� ��������Predicate
        // RoomCore�N���X�̔z�����
        // �������O�̕������邩�ǂ������m�F
        //==================================
        RoomCore r = System.Array.Find(roomInfo.GetRooms(), room => room.no == no);

        //���邩�ǂ����`�F�b�N
        if (r == null)
        {
            print(no + "�̔ԍ�����������������܂���ł���");
            print("���͂����ԍ����Ċm�F���Ă�������");
            return null;
        }

        // �������쐻
        GameObject room = Instantiate(r.room, new Vector3(0, 0, 0), Quaternion.identity);
        //�㉺���E�ɕ��������邩�ǂ����m�F

        Iscleared = roomDatas[nowH, nowW].isCleared;

        // �G�̐�������
        if (!roomDatas[nowH, nowW].isCleared)
        {
            Debug.Log("�G�𐶐����܂�");
            // �G�������W�b�N��ǉ�
        }
        else
        {
            Debug.Log("�N���A�ς݂̂��ߓG�͐�������܂���");
        }

        return room;
    }

    //=======================
    // �O���Q�Ɨp
    //=======================
    public RoomData[,] GetRoomDatas() { return roomDatas; }
}


