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

    //FadeOut後、次のシーンの名前
    [SerializeField]
    string NextScene_Name;

    //True: Fade_In  False: Fade_Out
    [SerializeField]
    bool IsFade_In;

    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0.0f;

        StartCoroutine(BeginTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFade_In == true)
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= 1.1f)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= transitiontime + 0.5f)
            {
                SceneManager.sceneLoaded += GameSceneLoaded;

                SceneManager.LoadScene(NextScene_Name);
            }
        }
    }

    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        // シーン切り替え後のスクリプトを取得
        //var gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        // データを渡す処理
        


        SceneManager.sceneLoaded -= GameSceneLoaded;
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
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
    }

}
