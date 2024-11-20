using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundCore
{
    [Tooltip("サウンドの名前")]
    public string name;

    //AudioSourceに必要な情報
    [Tooltip("サウンドの音源")]
    public AudioClip clip;
    [Tooltip("サウンドボリューム,0.0から1.0まで")]
    public float volume;
    [Tooltip("Loopするかどうか")]
    public bool loop;

}
