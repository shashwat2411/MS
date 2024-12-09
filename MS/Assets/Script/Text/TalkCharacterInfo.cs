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

//��������������������������������������������������������
//�@�@�J�X�^���C���X�y�N�^�[�p
//��������������������������������������������������������

[CustomEditor(typeof(TalkCharacterInfo))]
public class TalkCharacterInfoEditor : Editor
{
    private ReorderableList reorderableList; // ReorderableList���Ǘ�
    private bool[] foldoutStates; // �e�v�f�̊J��Ԃ��Ǘ�

    private void OnEnable()
    {
        SerializedProperty talkTextArray = serializedObject.FindProperty("talkText");

        // Foldout��Ԃ̏�����
        InitializeFoldoutStates(talkTextArray.arraySize);

        // ReorderableList�̏�����
        reorderableList = new ReorderableList(serializedObject, talkTextArray, true, true, true, true);

        // �w�b�_�[�̕`��
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "��b�ݒ�");
        };

        // �e�v�f�̕`��
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            // Foldout�̏�Ԃ�`��
            float foldoutOffset = 15f; // �{�^�����E�Ɉړ�����I�t�Z�b�g
            Rect foldoutRect = new Rect(rect.x + foldoutOffset, rect.y, rect.width - foldoutOffset, EditorGUIUtility.singleLineHeight);
            foldoutStates[index] = EditorGUI.Foldout(foldoutRect, foldoutStates[index], $"��b���e {index + 1}", true);

            if (foldoutStates[index])
            {
                DrawTextCoreProperties(element, rect);
            }
        };

        // �v�f�̍�����ݒ�
        reorderableList.elementHeightCallback = (int index) =>
        {
            if (!foldoutStates[index])
            {
                return EditorGUIUtility.singleLineHeight + 4; // �܂肽���ݎ��̍���
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
            foldoutStates[i] = false; // ������Ԃ͂��ׂĕ������
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

        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkName, new GUIContent("�����Ă���Ώ�"));
        yOffset += lineHeight;
        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkCharaImage, new GUIContent("�����Ă���Ώۂ̉摜"));
        yOffset += lineHeight;
        EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), talkInfo, new GUIContent("�����Ă�����e"));
        yOffset += lineHeight;

        // �I�����֘A�̕\��
        select.boolValue = EditorGUI.Toggle(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), "�I��������", select.boolValue);
        yOffset += lineHeight;
        if (select.boolValue)
        {
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), choices, new GUIContent("�I����"), true);
            yOffset += EditorGUI.GetPropertyHeight(choices, true) + 4; // �z��S�̂̍���
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), nextIndexes, new GUIContent("�I�������Ƃ̎��̃C���f�b�N�X"), true);
            yOffset += EditorGUI.GetPropertyHeight(nextIndexes, true) + 4;
        }

        // �ԍ��X�L�b�v�֘A�̕\��
        isCheck.boolValue = EditorGUI.Toggle(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), "�ԍ����΂�", isCheck.boolValue);
        yOffset += lineHeight;
        if (isCheck.boolValue)
        {
            EditorGUI.PropertyField(new Rect(rect.x, yOffset, rect.width, EditorGUIUtility.singleLineHeight), changeNumber, new GUIContent("��΂��ԍ�"));
        }
    }

    private float CalculateElementHeight(SerializedProperty element)
    {
        float height = EditorGUIUtility.singleLineHeight * 6 + 10; // ��{�I�ȍ���

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

        EditorGUILayout.LabelField($"���݂̗v�f��: {reorderableList.count}", EditorStyles.miniLabel);

        // JSON�t�@�C���C���|�[�g
        if (GUILayout.Button("��b���e�pJSON���C���|�[�g"))
        {
            string path = EditorUtility.OpenFilePanel("�C���|�[�g����JSON�t�@�C����I��", Application.dataPath, "json");
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
