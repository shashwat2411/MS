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
            SerializedProperty element = talkTextArray.GetArrayElementAtIndex(index);

            // Foldout�̏�Ԃ�`��
            float foldoutOffset = 15f; // �{�^�����E�Ɉړ�����I�t�Z�b�g
            Rect foldoutRect = new Rect(rect.x + foldoutOffset, rect.y, rect.width - foldoutOffset, EditorGUIUtility.singleLineHeight);
            foldoutStates[index] = EditorGUI.Foldout(foldoutRect, foldoutStates[index], $"��b���e {index}", true);

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

            SerializedProperty element = talkTextArray.GetArrayElementAtIndex(index);
            return CalculateElementHeight(element);
        };
    }

    private void InitializeFoldoutStates(int size)
    {
        foldoutStates = new bool[size];
        for (int i = 0; i < size; i++)
        {
            foldoutStates[i] = false; // ������Ԃ͂��ׂĊJ�������
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
        // �z�����\��
        EditorGUILayout.LabelField($"���݂̗v�f��: {reorderableList.count}", EditorStyles.miniLabel);
        serializedObject.ApplyModifiedProperties();
    }
}