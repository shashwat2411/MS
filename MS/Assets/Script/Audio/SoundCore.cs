using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundCore
{
    [Tooltip("�T�E���h�̖��O")]
    public string name;

    //AudioSource�ɕK�v�ȏ��
    [Tooltip("�T�E���h�̉���")]
    public AudioClip clip;
    [Tooltip("�T�E���h�{�����[��,0.0����1.0�܂�")]
    public float volume;
    [Tooltip("Loop���邩�ǂ���")]
    public bool loop;

}
