using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SoundManager : MonoBehaviour
{
  
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
              
                _instance = FindObjectOfType<SoundManager>();

                if (_instance == null)
                {
                   
                    GameObject go = new GameObject("SoundManager");
                    _instance = go.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

  
    private SoundManager() { }

    //AudioSource
    [SerializeField]
    public AudioSource bgmAudioSource;

    [SerializeField]
    public List<AudioSource> seAudioSources = new List<AudioSource>();

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
        if (s == null)
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

        foreach (AudioSource audioSource in seAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = s.clip;
                audioSource.volume = s.volume;
                audioSource.Play();
                return;
            }
        }

        Debug.LogWarning("No available AudioSource to play sound: " + name);

    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void StopAllSE()
    {
        foreach (var audioSource in seAudioSources)
        {
            audioSource.Stop();
        }
    }

    public void StopSE(string soundName)
    {

        SoundCore s = System.Array.Find(soundsInfo.GetSE(), sound => sound.name == soundName);
        // なければreturn
        if (s == null)
        {
           
            print(soundName + "という名前のSEは見つかりません。");
            print("登録している名前を確認して間違えていないか調べてください。");
            return;
        }

        foreach (AudioSource audioSource in seAudioSources)
        {
            if (audioSource.clip == s.clip && audioSource.isPlaying)
            {
                audioSource.Stop();
                return;
            }
        }


        Debug.LogWarning("Sound not found: " + soundName);

    }
}
