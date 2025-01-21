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

    [SerializeField] private Sprite defaultSprite; // ���I����Ԃ̉摜
    [SerializeField] private Sprite highlightedSprite; // �I����Ԃ̉摜

    private int nowNo = 0;
    private Coroutine displayCoroutine;
    private bool skipRequested = false; // �X�L�b�v�v���t���O
    private int currentChoiceIndex = 0; // ���ݑI�𒆂̑I�����C���f�b�N�X

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�̈ړ������Ȃǂ������ł���

        //�e�L�X�g����
        choicePanel.SetActive(false);
        _talkCharacterManager = FindObjectOfType<TalkCharacterManager>();
        if (_talkCharacterManager == null)
        {
            Debug.LogError("TalkCharacterManager���V�[���ɑ��݂��܂���B");
            return;
        }
        // ���݂̉�b�Ώۂ��擾
        nowText = _talkCharacterManager.nowTalker;
        SetText(nowText);

    }

    private void Update()
    {
        if (choicePanel.activeSelf) // �I�������\�����̂Ƃ�
        {
            HandleChoiceNavigation();
        }
        else
        {
            // �X�y�[�X�L�[�ŃX�L�b�v�����N�G�X�g
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skipRequested = true;
            }
        }
    }

    //=====================================
    //      ��b�V�X�e���̖{��
    //=====================================

    #region ��b�V�X�e��
    // ��b���J�n����
    public void SetText(TalkCharacterInfo temp)
    {
        nowText = temp;
        nowNo = 0;
        ShowNextDialogue();
    }

    // ���̉�b��\������
    private void ShowNextDialogue()
    {
        StopAllCoroutines(); // �O��̕\���������~
        TextCore tempText = nowText.GetNowText(nowNo);
        if (tempText == null) return;

        SetCharacterImage(tempText._talkCharaImage);


        talkNameText.text = tempText._talkName;

        //�I����������ꍇ
        if (tempText.select && tempText.choices != null && tempText.choices.Length > 0)
        {
            displayCoroutine = StartCoroutine(DisplaySelectText(tempText._talkInfo));
        }
        else
        {
            choicePanel.SetActive(false);
            // �ꕶ�����e�L�X�g��\�����鏈�����J�n
            if (tempText.isCheck) displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo, tempText.changeNumber));
            else displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo, nowNo + 1));

        }
    }

    //�I������\��
    private void ShowChoices(TextCore tempText)
    {
        choicePanel.SetActive(true);

        currentChoiceIndex = 0; // �ŏ��̑I������I����Ԃɂ���

        // �I�����𓮓I�ɕ\��
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < tempText.choices.Length)
            {
                // �{�^���̃e�L�X�g��ݒ�
                TMP_Text buttonText = choiceTexts[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = tempText.choices[i];
                choiceTexts[i].gameObject.SetActive(true);

                // �{�^���Ƀ��X�i�[��ݒ�
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

        // �ŏ��̑I�����������\��
        HighlightCurrentChoice();
    }

    //============================
    //  �ʏ��
    //============================

    // �ꕶ�����\�����A�X�L�b�v������ǉ�
    private IEnumerator DisplayText(string text, int num)
    {
        dialogueText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];

            // �������[�h
            float waitTime = Input.GetKey(KeyCode.LeftControl) ? 0.01f : 0.05f;
            yield return new WaitForSeconds(waitTime);

            // �X�y�[�X�L�[�őS���\��
            if (skipRequested)
            {
                dialogueText.text = text;
                break;
            }
        }

        // ���̉�b�ɐi�ޏ���

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitForSeconds(0.1f);
        skipRequested = false; //�X�L�b�v�����Z�b�g
        SetNextDialogue(num);
    }

    // ���̉�b�C���f�b�N�X��ݒ肷��
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
    //  �I������
    //============================

    private IEnumerator DisplaySelectText(string text)
    {
        dialogueText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];

            // �������[�h
            float waitTime = Input.GetKey(KeyCode.LeftControl) ? 0.01f : 0.05f;
            yield return new WaitForSeconds(waitTime);

            // �X�y�[�X�L�[�őS���\��
            if (skipRequested)
            {
                dialogueText.text = text;
                break;
            }
        }

        yield return new WaitForSeconds(0.3f);
        //���݂̖��O��\��
        ShowChoices(nowText.GetNowText(nowNo));

    }

    private void HandleChoiceNavigation()
    {
        // �㉺�L�[�őI�������ړ�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("��");
            UpdateChoiceIndex(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("��");
            UpdateChoiceIndex(1);
        }

        // �G���^�[�L�[�܂��̓X�y�[�X�L�[�őI�����m��
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmChoice();
        }
    }

    private void UpdateChoiceIndex(int direction)
    {
        // ���݂̃C���f�b�N�X���X�V
        int maxIndex = choiceTexts.Length - 1;
        currentChoiceIndex = Mathf.Clamp(currentChoiceIndex + direction, 0, maxIndex);

        // �I�����̋����\�����X�V
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
        // ���݂̑I�����ɑΉ�����A�N�V���������s
        if (currentChoiceIndex >= 0 && currentChoiceIndex < choiceTexts.Length)
        {
            choiceTexts[currentChoiceIndex].onClick.Invoke(); // �{�^���̃N���b�N�C�x���g���Ăяo��
        }
    }

    //============================
    //  �I������
    //============================

    // ��b�I������
    private void EndDialogue()
    {
        Destroy(gameObject);
        Debug.Log("��b���I�����A�I�u�W�F�N�g���폜����܂����B");
    }

    #endregion

    //���t�@�N�^�����O�ƃR�[�h����
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
