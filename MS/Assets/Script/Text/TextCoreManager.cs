using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextCoreManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/textcore_data.json";
        Debug.Log($"�f�[�^�ۑ��p�X: {savePath}");
    }

    // �f�[�^��ۑ�
    public void SaveData(List<TextCore> textCores)
    {
        string json = JsonUtility.ToJson(new TextCoreListWrapper { textCores = textCores }, true);
        File.WriteAllText(savePath, json);
        Debug.Log("�f�[�^��ۑ����܂���");
    }

    // �f�[�^��ǂݍ���
    public List<TextCore> LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("�ۑ��f�[�^��������܂���");
            return new List<TextCore>();
        }

        string json = File.ReadAllText(savePath);
        TextCoreListWrapper wrapper = JsonUtility.FromJson<TextCoreListWrapper>(json);
        Debug.Log("�f�[�^��ǂݍ��݂܂���");
        return wrapper.textCores;
    }

    // �f�[�^���폜
    public void DeleteData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("�f�[�^���폜���܂���");
        }
    }
}