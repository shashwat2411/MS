using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsInfo : MonoBehaviour
{
    //Sound�N���X�z��
    [SerializeField]
    private SoundCore[] bgm;

    [SerializeField]
    private SoundCore[] se;

    //�V���O���g����
    public static SoundsInfo instance;

    private void Awake()
    {
        //����Sound�}�l�[�W���[���������I�u�W�F�N�g���Ȃ��ꍇ
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);    //���̃I�u�W�F�N�g������
            return;
        }

        //���̃I�u�W�F�N�g�����[�h���Ă������Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    public SoundCore[] GetBGM() { return bgm;}
    public SoundCore[] GetSE() { return se; }
}
