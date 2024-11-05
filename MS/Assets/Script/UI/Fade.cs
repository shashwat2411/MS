using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    //Fade�p�}�e���A��
    [SerializeField]
    private Material _transitionIn;

    //Fade�J�n���犮���܂ł̎���
    float transitiontime = 1.0f;

    //Fade�̌o�ߎ���
    float ElapsedTime;

    //FadeOut��A���̃V�[���̖��O
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
        // �V�[���؂�ւ���̃X�N���v�g���擾
        //var gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        // �f�[�^��n������
        


        SceneManager.sceneLoaded -= GameSceneLoaded;
    }

    IEnumerator BeginTransition()
    {
        yield return Animate(_transitionIn, transitiontime);
    }

    /// <summary>
    /// time�b�����ăg�����W�V�������s��
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
