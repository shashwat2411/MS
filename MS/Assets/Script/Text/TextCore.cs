using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��b���e��ێ����邽�߂̊֐�
[System.Serializable]
public class TextCore
{

    [Tooltip("�����Ă���Ώ�")]
    public string _talkName = "";

    [Tooltip("�����Ă���Ώۂ̉摜")]
    public Sprite _talkCharaImage = null;

    [Tooltip("�����Ă�����e")]
    public string _talkInfo = "";

    [Tooltip("�I�������肩�Ȃ���")]
    public bool select = false;

    [Tooltip("�I�����̃e�L�X�g")]
    public string[] choices;

    [Tooltip("�I�������Ƃ̎��̃e�L�X�g�C���f�b�N�X")]
    public int[] nextIndexes;

    [Tooltip("�ԍ����΂������Ȃ�")]
    public bool isCheck = false;

    [Tooltip("��΂��Ȃ牽����")]
    public int changeNumber;

}
