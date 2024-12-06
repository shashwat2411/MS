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
    [SerializeField] private Button[] choiceTexts;

    [SerializeField] private Sprite defaultSprite; // 未選択状態の画像
    [SerializeField] private Sprite highlightedSprite; // 選択状態の画像

    private int nowNo = 0;
    private Coroutine displayCoroutine;
    private bool skipRequested = false; // スキップ要求フラグ
    private int currentChoiceIndex = 0; // 現在選択中の選択肢インデックス

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
        if (choicePanel.activeSelf) // 選択肢が表示中のとき
        {
            HandleChoiceNavigation();
        }
        else
        {
            // スペースキーでスキップをリクエスト
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skipRequested = true;
            }
        }
    }

    //=====================================
    //      会話システムの本体
    //=====================================

    #region 会話システム
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

        SetCharacterImage(tempText._talkCharaImage);


        talkNameText.text = tempText._talkName;

        //選択肢がある場合
        if (tempText.select && tempText.choices != null && tempText.choices.Length > 0)
        {
            displayCoroutine = StartCoroutine(DisplaySelectText(tempText._talkInfo));
        }
        else
        {
            choicePanel.SetActive(false);
            // 一文字ずつテキストを表示する処理を開始
            if (tempText.isCheck) displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo, tempText.changeNumber));
            else displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo, nowNo + 1));

        }
    }

    //選択肢を表示
    private void ShowChoices(TextCore tempText)
    {
        choicePanel.SetActive(true);

        currentChoiceIndex = 0; // 最初の選択肢を選択状態にする

        // 選択肢を動的に表示
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < tempText.choices.Length)
            {
                // ボタンのテキストを設定
                TMP_Text buttonText = choiceTexts[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = tempText.choices[i];
                choiceTexts[i].gameObject.SetActive(true);

                // ボタンにリスナーを設定
                int nextIndex = tempText.nextIndexes[i];
                choiceTexts[i].onClick.RemoveAllListeners();
                choiceTexts[i].onClick.AddListener(() =>
                {
                    SetNextDialogue(nextIndex);
                });
            }
            else
            {
                choiceTexts[i].gameObject.SetActive(false);
            }
        }

        // 最初の選択肢を強調表示
        HighlightCurrentChoice();
    }

    //============================
    //  通常版
    //============================

    // 一文字ずつ表示し、スキップ処理を追加
    private IEnumerator DisplayText(string text, int num)
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
        SetNextDialogue(num);
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

    //============================
    //  選択肢版
    //============================

    private IEnumerator DisplaySelectText(string text)
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

        yield return new WaitForSeconds(0.3f);
        //現在の名前を表示
        ShowChoices(nowText.GetNowText(nowNo));

    }

    private void HandleChoiceNavigation()
    {
        // 上下キーで選択肢を移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("↑");
            UpdateChoiceIndex(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("↓");
            UpdateChoiceIndex(1);
        }

        // エンターキーまたはスペースキーで選択を確定
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmChoice();
        }
    }

    private void UpdateChoiceIndex(int direction)
    {
        // 現在のインデックスを更新
        int maxIndex = choiceTexts.Length - 1;
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex + direction, 0, maxIndex);

        // 選択肢の強調表示を更新
        HighlightCurrentChoice();
    }

    private void HighlightCurrentChoice()
    {
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            var button = choiceTexts[i];
            var image = button.GetComponent<Image>();

            image.sprite = (i == currentChoiceIndex) ? highlightedSprite : defaultSprite;
            
        }
    }

    private void ConfirmChoice()
    {
        // 現在の選択肢に対応するアクションを実行
        if (currentChoiceIndex >= 0 && currentChoiceIndex < choiceTexts.Length)
        {
            choiceTexts[currentChoiceIndex].onClick.Invoke(); // ボタンのクリックイベントを呼び出す
        }
    }

    //============================
    //  終了処理
    //============================

    // 会話終了処理
    private void EndDialogue()
    {
        Destroy(gameObject);
        Debug.Log("会話が終了し、オブジェクトが削除されました。");
    }

    #endregion

    //リファクタリングとコード整理
    private void SetCharacterImage(Sprite image)
    {
        if (image != null)
        {
            characterImage.sprite = image;
            Color tempColor = characterImage.color;
            tempColor.a = 1;
            characterImage.color = tempColor;
        }
        else
        {
            characterImage.sprite = null;
            Color tempColor = characterImage.color;
            tempColor.a = 0;
            characterImage.color = tempColor;
        }
    }


}
