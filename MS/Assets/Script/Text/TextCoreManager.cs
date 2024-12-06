using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextCoreManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/textcore_data.json";
        Debug.Log($"データ保存パス: {savePath}");
    }

    // データを保存
    public void SaveData(List<TextCore> textCores)
    {
        string json = JsonUtility.ToJson(new TextCoreListWrapper { textCores = textCores }, true);
        File.WriteAllText(savePath, json);
        Debug.Log("データを保存しました");
    }

    // データを読み込む
    public List<TextCore> LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("保存データが見つかりません");
            return new List<TextCore>();
        }

        string json = File.ReadAllText(savePath);
        TextCoreListWrapper wrapper = JsonUtility.FromJson<TextCoreListWrapper>(json);
        Debug.Log("データを読み込みました");
        return wrapper.textCores;
    }

    // データを削除
    public void DeleteData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("データを削除しました");
        }
    }
}