using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TalkCharacterInfo : MonoBehaviour
{
    public TextCore[] talkText;

    private void Start()
    {
        
    }

    public TextCore GetNowText(int i) { return talkText[i]; }
    public int GetTextLangth() { return talkText.Length; }
}

//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
//　　カスタムインスペクター用
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝

[CustomEditor(typeof(TalkCharacterInfo))]
public class TalkCharacterInfoEditor : Editor
{
    private ReorderableList reorderableList; // ReorderableListを管理
    private bool[] foldoutStates; // 各要素の開閉状態を管理

    private void OnEnable()
    {
        SerializedProperty talkTextArray = serializedObject.FindProperty("talkText");

        // Foldout状態の初期化
        InitializeFoldoutStates(talkTextArray.arraySize);

        // ReorderableListの初期化
        reorderableList = new ReorderableList(serializedObject, talkTextArray, true, true, true, true);

        // ヘッダーの描画
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "会話設定");
        };

        // 各要素の描画
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            // Foldoutの状態を描画
            float foldoutOffset = 15f; // ボタンを右に移動するオフセット
            Rect foldoutRect = new Rect(rect.x + foldoutOffset, rect.y, rect.width - foldoutOffset, EditorGUIUtility.singleLineHeight);
            foldoutStates[index] = EditorGUI.Foldout(foldoutRect, foldoutStates[index], $"会話内容 {index + 1}", true);

            if (foldoutStates[index])
            {
                DrawTextCoreProperties(element, rect);
            }
        };

        // 要素の高さを設定
        reorderableList.elementHeightCallback = (int index) =>
        {
            if (!foldoutStates[index])
            {
                return EditorGUIUtility.singleLineHeight + 4; // 折りたたみ時の高さ
            }

            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return CalculateElementHeight(element);
        };
    }

    private void InitializeFoldoutStates(int size)
    {
        foldoutStates = new bool[size];
        for (int i = 0; i < size; i++)
        {
            foldoutStates[i] = false; // 初期状態はすべて閉じた状態
        }
    }

    private void DrawTextCoreProperties(SerializedProperty element, Rect rect)
    {
        float yOffset = rect.y + EditorGUIUtility.singleLineHeight + 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 2;

        SerializedProperty talkName = element.FindPropertyRelative("_talkName");
        SerializedProperty talkCharaImage = element.FindPropertyRelative("_talkCharaImage");
        SerializedProperty talkInfo = element.FindPropertyRelative("_talkInfo");
        SerializedProperty select = element.FindPropertyRelative("select");
        SerializedProperty choices = element.FindPropertyRelative("choices");
        SerializedProperty nextIndexes = element.FindPropertyRelative("nextIndexes");
        SerializedProperty isCheck = element.FindPropertyRelative("isCheck");
        SerializedProperty changeNumber = element.FindPropertyRelative("changeNumber");

        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkName, new GUIContent("喋っている対象"));
        yOffset += lineHeight;
        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkCharaImage, new GUIContent("喋っている対象の画像"));
        yOffset += lineHeight;
        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkInfo, new GUIContent("喋っている内容"));
        yOffset += lineHeight;

        // 選択肢関連の表示
        select.boolValue = EditorGUI.Toggle(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), "選択肢あり", select.boolValue);
        yOffset += lineHeight;
        if (select.boolValue)
        {
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), choices, new GUIContent("選択肢"), true);
            yOffset += EditorGUI.GetPropertyHeight(choices, true) + 4; // 配列全体の高さ
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), nextIndexes, new GUIContent("選択肢ごとの次のインデックス"), true);
            yOffset += EditorGUI.GetPropertyHeight(nextIndexes, true) + 4;
        }

        // 番号スキップ関連の表示
        isCheck.boolValue = EditorGUI.Toggle(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), "番号を飛ばす", isCheck.boolValue);
        yOffset += lineHeight;
        if (isCheck.boolValue)
        {
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), changeNumber, new GUIContent("飛ばす番号"));
        }
    }

    private float CalculateElementHeight(SerializedProperty element)
    {
        float height = EditorGUIUtility.singleLineHeight * 6 + 10; // 基本的な高さ

        SerializedProperty select = element.FindPropertyRelative("select");
        SerializedProperty choices = element.FindPropertyRelative("choices");
        SerializedProperty nextIndexes = element.FindPropertyRelative("nextIndexes");
        SerializedProperty isCheck = element.FindPropertyRelative("isCheck");

        if (select.boolValue)
        {
            height += EditorGUI.GetPropertyHeight(choices, true) + 4;
            height += EditorGUI.GetPropertyHeight(nextIndexes, true) + 4;
        }
        if (isCheck.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight + 2;
        }

        return height;
    }

    public override void OnInspectorGUI()
    {
        reorderableList.DoLayoutList();

        EditorGUILayout.LabelField($"現在の要素数: {reorderableList.count}", EditorStyles.miniLabel);

        // JSONファイルインポート
        if (GUILayout.Button("会話内容用JSONをインポート"))
        {
            string path = EditorUtility.OpenFilePanel("インポートするJSONファイルを選択", Application.dataPath, "json");
            if (!string.IsNullOrEmpty(path))
            {
                string json = File.ReadAllText(path);
                TextCore[] loadedData = JsonUtility.FromJson<TextCoreListWrapper>(json).textCores.ToArray();
                ApplyLoadedData(loadedData);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ApplyLoadedData(TextCore[] loadedData)
    {
        SerializedProperty talkTextArray = serializedObject.FindProperty("talkText");
        talkTextArray.ClearArray();
        foreach (var core in loadedData)
        {
            talkTextArray.InsertArrayElementAtIndex(talkTextArray.arraySize);
            SerializedProperty element = talkTextArray.GetArrayElementAtIndex(talkTextArray.arraySize - 1);
            element.FindPropertyRelative("_talkName").stringValue = core._talkName;
            element.FindPropertyRelative("_talkCharaImage").objectReferenceValue = core._talkCharaImage;
            element.FindPropertyRelative("_talkInfo").stringValue = core._talkInfo;
            element.FindPropertyRelative("select").boolValue = core.select;
            element.FindPropertyRelative("choices").arraySize = core.choices.Length;
            for (int i = 0; i < core.choices.Length; i++)
            {
                element.FindPropertyRelative("choices").GetArrayElementAtIndex(i).stringValue = core.choices[i];
            }
            element.FindPropertyRelative("nextIndexes").arraySize = core.nextIndexes.Length;
            for (int i = 0; i < core.nextIndexes.Length; i++)
            {
                element.FindPropertyRelative("nextIndexes").GetArrayElementAtIndex(i).intValue = core.nextIndexes[i];
            }
            element.FindPropertyRelative("isCheck").boolValue = core.isCheck;
            element.FindPropertyRelative("changeNumber").intValue = core.changeNumber;
        }
    }
}
