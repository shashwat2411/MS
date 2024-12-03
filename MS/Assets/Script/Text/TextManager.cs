using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

public class TextManager : MonoBehaviour
{
    private TalkCharacterManager _talkCharacterManager;
    private TalkCharacterInfo nowText;

    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text talkNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private TMP_Text[] choiceTexts;

    private int nowNo = 0;
    private Coroutine displayCoroutine;
    private bool skipRequested = false; // スキップ要求フラグ

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーの移動処理などをここできる

        //テキスト処理
        choicePanel.SetActive(false);
        _talkCharacterManager = FindObjectOfType<TalkCharacterManager>();
        if (_talkCharacterManager == null)
        {
            Debug.LogError("TalkCharacterManagerがシーンに存在しません。");
            return;
        }
        // 現在の会話対象を取得
        nowText = _talkCharacterManager.nowTalker;
        SetText(nowText);

    }

    private void Update()
    {
        // スペースキーでスキップをリクエスト
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
        }
    }

    // 会話を開始する
    public void SetText(TalkCharacterInfo temp)
    {
        nowText = temp;
        nowNo = 0;
        ShowNextDialogue();
    }

    // 次の会話を表示する
    private void ShowNextDialogue()
    {
        StopAllCoroutines(); // 前回の表示処理を停止
        TextCore tempText = nowText.GetNowText(nowNo);
        if (tempText == null) return;

        // キャラクター画像と名前を更新
        if (tempText._talkCharaImage != null)
        {
            characterImage.sprite = tempText._talkCharaImage;
            Color tempColor = characterImage.color;
            tempColor.a = 255;
            characterImage.color = tempColor;
        }

        else
        {
            characterImage.sprite = null;
            Color tempColor = characterImage.color;
            tempColor.a = 0;
            characterImage.color = tempColor;
        }
        

        talkNameText.text = tempText._talkName;

        // 一文字ずつテキストを表示する処理を開始
        displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo));
    }

    // 一文字ずつ表示し、スキップ処理を追加
    private IEnumerator DisplayText(string text)
    {
        dialogueText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];

            // 高速モード
            float waitTime = Input.GetKey(KeyCode.LeftControl) ? 0.01f : 0.05f;
            yield return new WaitForSeconds(waitTime);

            // スペースキーで全文表示
            if (skipRequested)
            {
                dialogueText.text = text;
                break; 
            }
        }

        // 次の会話に進む準備
        
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(0.1f);
        skipRequested = false; //スキップをリセット
        SetNextDialogue(nowNo + 1);
    }

    // 次の会話インデックスを設定する
    private void SetNextDialogue(int index)
    {
        nowNo = index;

        if (nowNo >= nowText.GetTextLangth() || index == 999)
        {
            EndDialogue();
        }
        else
        {
            ShowNextDialogue();
        }
    }

    // 会話終了処理
    private void EndDialogue()
    {
        Destroy(gameObject);
        Debug.Log("会話が終了し、オブジェクトが削除されました。");
    }
}
