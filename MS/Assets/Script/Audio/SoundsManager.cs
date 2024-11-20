using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundsManager : MonoBehaviour
{
    //AudioSource
    [SerializeField]
    public AudioSource bgmAudioSource;
    [SerializeField]
    public AudioSource seAudioSource;

    //�T�E���h����ǂݍ���
    [SerializeField]
    public SoundsInfo soundsInfo;

    public string nowBGMName = "defaultBGM";
    public string nowSEName = "defaultSE";

    public void PlayBGM(string name)
    {
        // �����_�� ��������Predicate
        // SoundCore�N���X�̔z�����
        // �������O�̕������邩�ǂ������m�F
        SoundCore s = System.Array.Find(soundsInfo.GetBGM(), sound => sound.name == name);
        //������Ζ߂�
        if(s == null)
        {
            print(name + "�Ƃ������O��BGM�͌�����܂���B");
            print("�o�^���Ă��閼�O���m�F���ĊԈႦ�Ă��Ȃ������ׂĂ��������B");
            return;
        }

        bgmAudioSource.clip = s.clip;
        bgmAudioSource.volume = s.volume;
        bgmAudioSource.loop = s.loop;

        bgmAudioSource.Play();
    }
    
    public void PlaySE(string name) 
    {
        // �����_���@��������Predicate
        // Sound�N���X�̔z��̒��̖��O�ɁC
        // ����name�ɓ��������̂����邩�ǂ����m�F
        SoundCore s = System.Array.Find(soundsInfo.GetSE(), sound => sound.name == name);
        // �Ȃ����return
        if (s == null)
        {
            print(name + "�Ƃ������O��SE�͌�����܂���B");
            print("�o�^���Ă��閼�O���m�F���ĊԈႦ�Ă��Ȃ������ׂĂ��������B");
            return;
        }

        seAudioSource.clip = s.clip;
        seAudioSource.volume = s.volume;

        // �����Play()
        seAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
}

//�J�X�^��
#if UNITY_EDITOR
[CustomEditor(typeof(SoundsManager))]
class CustomSoundManager : Editor
{
    #region �J�X�^��
    private int selectTab = 0;
    private string[] tabTitles =
    {
        "��{���", "BGM", "SE"
    };

    private bool basic = false;
    private bool isBGM = false;
    private bool isSE = false;
    #endregion

    public override void OnInspectorGUI()
    {
        //�^�[�Q�b�g�ݒ�
        var CI = (SoundsManager)target;
        if (CI == null) return;

        selectTab = GUILayout.Toolbar(selectTab, tabTitles);

        GUILayout.Space(5);
        DrawDottedLine(Color.gray);
        GUILayout.Space(5);

        switch (selectTab)
        {
            case 0:
                ShowBasicInfo(CI);
                break;
            case 1:
                ShowBGMInfo(CI);
                break;
            case 2:
                ShowSEInfo(CI);
                break;
        }



        GUILayout.Space(5);
        DrawDottedLine(Color.gray);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("PlayBGM"))
        {
            CI.PlayBGM(CI.nowBGMName);
            Debug.Log(CI.nowBGMName + "���Đ����܂�");
        }
        if (GUILayout.Button("PlaySE"))
        {
            CI.PlaySE(CI.nowSEName);
            Debug.Log(CI.nowSEName + "���Đ����܂�");
        }
        if (GUILayout.Button("StopBGM"))
        {
            CI.StopBGM();
            Debug.Log("BGM���~���܂�");
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ShowBasicInfo(SoundsManager SM)
    {
        Undo.RecordObject(SM, "SoundMaanager���ύX����܂���");

        //���x����\��
        GUIStyle centerdStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("���C���ݒ�ł�");

        //�{�b�N�X��\��
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        //���݂̐ݒ��\��
        GUILayout.Label("���݂�BGM:" + SM.nowBGMName);
        GUILayout.Label("���݂�SE :" + SM.nowSEName);

        //AudioSource�����邽�߂̃{�b�N�X
        // AudioSource�����邽�߂̃t�B�[���h
        SM.bgmAudioSource = (AudioSource)EditorGUILayout.ObjectField("BGM AudioSource", SM.bgmAudioSource, typeof(AudioSource), true);
        SM.seAudioSource = (AudioSource)EditorGUILayout.ObjectField("SE AudioSource", SM.seAudioSource, typeof(AudioSource), true);

        // SoundInfo�����邽�߂̃t�B�[���h
        SM.soundsInfo = (SoundsInfo)EditorGUILayout.ObjectField("Sounds Info", SM.soundsInfo, typeof(SoundsInfo), true);

        EditorGUILayout.EndVertical();
    }

    private void ShowBGMInfo(SoundsManager SM)
    {
        if(SM.soundsInfo == null)
        {
            EditorGUILayout.HelpBox("Sounds Indo is not asigned", MessageType.Warning);
            return;
        }

        //���݂̐ݒ��\��
        GUILayout.Label("���݂�BGM:" + SM.nowBGMName);

        //�{�^����\��������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        SoundCore[] bgm = SM.soundsInfo.GetBGM();
        if(bgm.Length == 0)
        {
            EditorGUILayout.HelpBox("No BGM found.", MessageType.Info);
        }
        
        short i = 0; //���̗v�f��
        foreach (var sound in bgm)
        {
            if(i == 0){ EditorGUILayout.BeginHorizontal(); }
            i++;

            if(GUILayout.Button(sound.name)){
                SM.nowBGMName = sound.name;
            }

            if (i == 3){ 
                EditorGUILayout.EndHorizontal();
                i -= 3;
            }
            
        }

        // �Ō�̍s���I����Ă��Ȃ���ΏI��
        if (i != 0)
        {
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void ShowSEInfo(SoundsManager SM)
    {
        if (SM.soundsInfo == null)
        {
            EditorGUILayout.HelpBox("Sounds Indo is not asigned", MessageType.Warning);
            return;
        }

        //���݂̐ݒ��\��
        GUILayout.Label("���݂�SE:" + SM.nowSEName);

        //�{�^����\��������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        SoundCore[] se = SM.soundsInfo.GetSE();
        if (se.Length == 0)
        {
            EditorGUILayout.HelpBox("No BGM found.", MessageType.Info);
        }

        short i = 0; //���̗v�f��
        foreach (var sound in se)
        {
            if (i == 0) { EditorGUILayout.BeginHorizontal(); }
            i++;

            if (GUILayout.Button(sound.name))
            {
                SM.nowSEName = sound.name;
            }

            if (i == 3)
            {
                EditorGUILayout.EndHorizontal();
                i -= 3;
            }

        }

        // �Ō�̍s���I����Ă��Ȃ���ΏI��
        if (i != 0)
        {
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

    }

    #region CustomUI
    //��؂��(�����𒲐�)
    private void DrawUILine(Color color, int thickness = 1, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(thickness + padding));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    //�_���`��
    private void DrawDottedLine(Color color, int thickness = 2, int segmentLength = 5, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(thickness + padding));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;

        float x = r.x;
        while (x < r.x + r.width)
        {
            EditorGUI.DrawRect(new Rect(x, r.y, segmentLength, thickness), color);
            x += segmentLength * 2;
        }
    }

    #endregion

}

#endif
