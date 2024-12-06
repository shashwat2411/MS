using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

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
            SerializedProperty element = talkTextArray.GetArrayElementAtIndex(index);

            // Foldoutの状態を描画
            float foldoutOffset = 15f; // ボタンを右に移動するオフセット
            Rect foldoutRect = new Rect(rect.x + foldoutOffset, rect.y, rect.width - foldoutOffset, EditorGUIUtility.singleLineHeight);
            foldoutStates[index] = EditorGUI.Foldout(foldoutRect, foldoutStates[index], $"会話内容 {index}", true);

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

            SerializedProperty element = talkTextArray.GetArrayElementAtIndex(index);
            return CalculateElementHeight(element);
        };
    }

    private void InitializeFoldoutStates(int size)
    {
        foldoutStates = new bool[size];
        for (int i = 0; i < size; i++)
        {
            foldoutStates[i] = false; // 初期状態はすべて開いた状態
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
        // 配列情報を表示
        EditorGUILayout.LabelField($"現在の要素数: {reorderableList.count}", EditorStyles.miniLabel);
        serializedObject.ApplyModifiedProperties();
    }
}