using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    instance = go.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }


    private AudioSource bgmAudioSource;
    private AudioSource seAudioSource;

 
    public AudioClip[] soundEffects;

    private Dictionary<string, AudioClip> soundEffectsDic = new Dictionary<string, AudioClip>();
    public float bgmVolume = 0.5f;
    public float seVolume = 1.0f;

    void Awake()
    {
        
        DontDestroyOnLoad(gameObject);

       
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true; 
        bgmAudioSource.volume = bgmVolume;

        seAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSource.volume = seVolume;

        foreach(var se in soundEffects)
        {
            AddSoundEffect(se.name, se);
        }
       
    }

    // Play Bgm
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmAudioSource.isPlaying && bgmAudioSource.clip == bgmClip)
            return; 

        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

 
    public void PlaySE(int index)
    {
        if (index < 0 || index >= soundEffects.Length)
        {
            Debug.LogError("Invalid sound effect index.");
            return;
        }
        seAudioSource.PlayOneShot(soundEffects[index], seVolume);
    }

    public void PlaySE(string name)
    {
        if (!soundEffectsDic.ContainsKey(name))
        {
            Debug.LogError("Invalid sound effect index.");
            return;
        }
        seAudioSource.PlayOneShot(soundEffectsDic[name], seVolume);
    }


    public void AddSoundEffect(string name, AudioClip clip)
    {
        if (!soundEffectsDic.ContainsKey(name))
        {
            soundEffectsDic.Add(name, clip);
        }
        else
        {
            Debug.LogWarning($"Sound effect with the name '{name}' already exists.");
        }
    }



    // BGM音量
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 1f);
        bgmAudioSource.volume = bgmVolume;
    }

    // SE音量
    public void SetSEVolume(float volume)
    {
        seVolume = Mathf.Clamp(volume, 0f, 1f);
        seAudioSource.volume = seVolume;
    }

}
