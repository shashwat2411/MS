using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private Animator animator;

    public Image fadeImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FadeIn();
    }

    private void FadeIn()
    {
        animator.SetBool("fadeIn", true);
        animator.SetBool("fadeOut", false);
    }

    public void FadeOut()
    {
        animator.SetBool("fadeIn", false);
        animator.SetBool("fadeOut", true);
    }

    public void FadeInOver()
    {
        animator.SetBool("fadeIn", false);
        SoundManager.Instance.PlayBGM("BGM_1");
    }
    public void FadeOutOver()
    {
        animator.SetBool("fadeIn", false);
        SoundManager.Instance.StopBGM();
    }
}
