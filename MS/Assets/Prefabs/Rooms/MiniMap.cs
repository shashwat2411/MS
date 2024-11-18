using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour
{
    public GameObject roomIconPrefab;       // �����A�C�R����Prefab
    public RectTransform miniMapContainer; // �~�j�}�b�v��UI�R���e�i

    public RoomData[,] roomDatas;
    public float roomIconSize = 50f; // �����A�C�R���̃T�C�Y

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
                if (roomType == 0) continue; // ���������݂��Ȃ��ꍇ�̓X�L�b�v

                //���������݂���ꍇ
                GameObject icon = Instantiate(roomIconPrefab, miniMapContainer);
                RectTransform rect = icon.GetComponent<RectTransform>();

                // �A�C�R���̈ʒu��ݒ�
                rect.anchoredPosition = new Vector2(x * roomIconSize, -y * roomIconSize);

                // ���ʂȕ����̃A�C�R���ɐF��t���� 
                // ����̈ʒu�ɃA�C�R�����쐻����
                if (roomType == 1) icon.GetComponent<Image>().color = Color.green; // �X�^�[�g�n�_
                if (roomType == 99) icon.GetComponent<Image>().color = Color.red; // �S�[���n�_
            }
        }
    }
}
