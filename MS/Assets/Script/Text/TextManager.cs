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
    private bool skipRequested = false; // �X�L�b�v�v���t���O

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
        // �X�y�[�X�L�[�ŃX�L�b�v�����N�G�X�g
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
        }
    }

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

        // �L�����N�^�[�摜�Ɩ��O���X�V
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

        // �ꕶ�����e�L�X�g��\�����鏈�����J�n
        displayCoroutine = StartCoroutine(DisplayText(tempText._talkInfo));
    }

    // �ꕶ�����\�����A�X�L�b�v������ǉ�
    private IEnumerator DisplayText(string text)
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
        SetNextDialogue(nowNo + 1);
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

    // ��b�I������
    private void EndDialogue()
    {
        Destroy(gameObject);
        Debug.Log("��b���I�����A�I�u�W�F�N�g���폜����܂����B");
    }
}
