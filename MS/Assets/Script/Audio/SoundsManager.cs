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

    //サウンド情報を読み込む
    [SerializeField]
    public SoundsInfo soundsInfo;

    public string nowBGMName = "defaultBGM";
    public string nowSEName = "defaultSE";

    public void PlayBGM(string name)
    {
        // ラムダ式 第二引数はPredicate
        // SoundCoreクラスの配列内に
        // 同じ名前の物があるかどうかを確認
        SoundCore s = System.Array.Find(soundsInfo.GetBGM(), sound => sound.name == name);
        //無ければ戻る
        if(s == null)
        {
            print(name + "という名前のBGMは見つかりません。");
            print("登録している名前を確認して間違えていないか調べてください。");
            return;
        }

        bgmAudioSource.clip = s.clip;
        bgmAudioSource.volume = s.volume;
        bgmAudioSource.loop = s.loop;

        bgmAudioSource.Play();
    }
    
    public void PlaySE(string name) 
    {
        // ラムダ式　第二引数はPredicate
        // Soundクラスの配列の中の名前に，
        // 引数nameに等しいものがあるかどうか確認
        SoundCore s = System.Array.Find(soundsInfo.GetSE(), sound => sound.name == name);
        // なければreturn
        if (s == null)
        {
            print(name + "という名前のSEは見つかりません。");
            print("登録している名前を確認して間違えていないか調べてください。");
            return;
        }

        seAudioSource.clip = s.clip;
        seAudioSource.volume = s.volume;

        // あればPlay()
        seAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void StopSE()
    {
        seAudioSource.Stop();
    }
}



//カスタム
#if UNITY_EDITOR
[CustomEditor(typeof(SoundsManager))]
class CustomSoundManager : Editor
{
    #region カスタム
    private int selectTab = 0;
    private string[] tabTitles =
    {
        "基本情報", "BGM", "SE"
    };

    private bool basic = false;
    private bool isBGM = false;
    private bool isSE = false;
    #endregion

    public override void OnInspectorGUI()
    {
        //ターゲット設定
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
            Debug.Log(CI.nowBGMName + "を再生します");
        }
        if (GUILayout.Button("PlaySE"))
        {
            CI.PlaySE(CI.nowSEName);
            Debug.Log(CI.nowSEName + "を再生します");
        }
        if (GUILayout.Button("StopBGM"))
        {
            CI.StopBGM();
            Debug.Log("BGMを停止します");
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ShowBasicInfo(SoundsManager SM)
    {
        Undo.RecordObject(SM, "SoundMaanagerが変更されました");

        //ラベルを表示
        GUIStyle centerdStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("メイン設定です");

        //ボックスを表示
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        //現在の設定を表示
        GUILayout.Label("現在のBGM:" + SM.nowBGMName);
        GUILayout.Label("現在のSE :" + SM.nowSEName);

        //AudioSourceを入れるためのボックス
        // AudioSourceを入れるためのフィールド
        SM.bgmAudioSource = (AudioSource)EditorGUILayout.ObjectField("BGM AudioSource", SM.bgmAudioSource, typeof(AudioSource), true);
        SM.seAudioSource = (AudioSource)EditorGUILayout.ObjectField("SE AudioSource", SM.seAudioSource, typeof(AudioSource), true);

        // SoundInfoを入れるためのフィールド
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

        //現在の設定を表示
        GUILayout.Label("現在のBGM:" + SM.nowBGMName);

        //ボタンを表示させる
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        SoundCore[] bgm = SM.soundsInfo.GetBGM();
        if(bgm.Length == 0)
        {
            EditorGUILayout.HelpBox("No BGM found.", MessageType.Info);
        }
        
        short i = 0; //一列の要素数
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

        // 最後の行が終わっていなければ終了
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

        //現在の設定を表示
        GUILayout.Label("現在のSE:" + SM.nowSEName);

        //ボタンを表示させる
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        SoundCore[] se = SM.soundsInfo.GetSE();
        if (se.Length == 0)
        {
            EditorGUILayout.HelpBox("No BGM found.", MessageType.Info);
        }

        short i = 0; //一列の要素数
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

        // 最後の行が終わっていなければ終了
        if (i != 0)
        {
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

    }

    #region CustomUI
    //区切り線(太さを調整)
    private void DrawUILine(Color color, int thickness = 1, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(thickness + padding));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    //点線描画
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
