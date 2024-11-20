using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    //Fade用マテリアル
    [SerializeField]
    private Material _transitionIn;

    //Fade開始から完了までの時間
    float transitiontime = 1.0f;

    //Fadeの経過時間
    float ElapsedTime;

    

    //True: Fade_In  False: Fade_Out
    [SerializeField]
    bool IsFade_In;

    public bool Fade_End;

    // Start is called before the first frame update
    void Start()
    {
        Fade_End = false;

        ElapsedTime = 0.0f;

        StartCoroutine(BeginTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFade_In == true)//fadein
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= transitiontime + 0.5f)
            {
                Fade_End = true;
                this.gameObject.SetActive(false);
            }
        }
        else//fadeout
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= transitiontime + 0.5f)
            {

                //this.gameObject.SetActive(false);
                Fade_End = true;
            }
        }
    }

    public void FadeReset()
    {
        Fade_End = false;

        ElapsedTime = 0.0f;

        StartCoroutine(BeginTransition());
    }


    IEnumerator BeginTransition()
    {
        yield return Animate(_transitionIn, transitiontime);
    }

    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Animate(Material material, float time)
    {
        GetComponent<Image>().material = material;
        float current = 0;

        if (IsFade_In == true)
        {
            while (current < time)
            {
                material.SetFloat("_Alpha",1- current / time);
                yield return new WaitForEndOfFrame();
                current += Time.deltaTime;
            }
            material.SetFloat("_Alpha", 0);
        }
        else
        {
            while (current < time)
            {
                material.SetFloat("_Alpha", current / time);
                yield return new WaitForEndOfFrame();
                current += Time.deltaTime;
            }
            material.SetFloat("_Alpha", 1);
        } 
    }
}
