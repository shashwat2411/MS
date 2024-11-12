using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsInfo : MonoBehaviour
{
    //Soundクラス配列
    [SerializeField]
    private SoundCore[] bgm;

    [SerializeField]
    private SoundCore[] se;

    //シングルトン化
    public static SoundsInfo instance;

    private void Awake()
    {
        //もしSoundマネージャーが入ったオブジェクトがない場合
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);    //このオブジェクトを消す
            return;
        }

        //このオブジェクトをロードしても消さないようにする
        DontDestroyOnLoad(gameObject);
    }

    public SoundCore[] GetBGM() { return bgm;}
    public SoundCore[] GetSE() { return se; }
}
