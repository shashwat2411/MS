using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //������Pause�A�V�[���J�ڂȂǂ̊Ǘ�
    [SerializeField]
    GameObject FadeIn, FadeOut;

    bool SceneChangeflag;

    //�V�[���J�ڂł̃f�[�^�󂯓n��


    //FadeOut��A���̃V�[���̖��O
    [SerializeField]
    string NextScene_Name;

    // Start is called before the first frame update
    void Start()
    {
       
        SceneChangeflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeOut.GetComponent<Fade>().Fade_End == true && SceneChangeflag == true) 
        {
            // SceneManager.sceneLoaded += GameSceneLoaded;

            SceneManager.LoadScene(NextScene_Name);
            Debug.Log("1");
        }
    }

   

    public void SceneChange()
    {
       // FadeIn.GetComponent<Fade>().FadeReset();
        FadeOut.SetActive(true);

        SceneChangeflag = true;
    }


    public void ReturnTitle()
    {
        // FadeIn.GetComponent<Fade>().FadeReset();
        FadeOut.SetActive(true);

        SceneChangeflag = true;

        NextScene_Name = "MainMenu";
    }

    public void StageFadeIn()
    {
        FadeIn.GetComponent<Fade>().FadeReset();
        FadeOut.SetActive(false);
        FadeIn.SetActive(true);
    }

    public void StageFadeOut()
    {
        FadeIn.GetComponent<Fade>().FadeReset();
        FadeOut.SetActive(true);
    }

    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        // �V�[���؂�ւ���̃X�N���v�g���擾
        //var gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        // �f�[�^��n������



        SceneManager.sceneLoaded -= GameSceneLoaded;
    }


}
