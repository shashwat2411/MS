using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class TalkCharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePrefab; // TextManagerを含むプレハブ
    public TalkCharacterInfo nowTalker;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TalkStart(TalkCharacterInfo talker)
    {
        // プレハブを生成
        GameObject dialogueInstance = Instantiate(dialoguePrefab, transform);
        nowTalker = talker;
        TextManager textManager = dialogueInstance.GetComponent<TextManager>();
        textManager.SetText(nowTalker);
    }
}
