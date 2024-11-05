using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pause : MonoBehaviour
{
    static List<Pause> targets = new List<Pause>();   // �|�[�Y�Ώۂ̃X�N���v�g

    // �|�[�Y�Ώۂ̃R���|�[�l���g
    Behaviour[] pauseBehavs = null;

    Rigidbody[] rgBodies = null;
    Vector3[] rgBodyVels = null;
    Vector3[] rgBodyAVels = null;


    // Start is called before the first frame update
    void Start()
    {
        pauseBehavs = null;
        rgBodies = null;
        rgBodyVels = null;
        rgBodyAVels = null;



        // �|�[�Y�Ώۂɒǉ�����
        targets.Add(this);
    }


    // �j�������Ƃ�
    void OnDestory()
    {
        // �|�[�Y�Ώۂ��珜�O����
        targets.Remove(this);
    }

    // �|�[�Y���ꂽ�Ƃ�
    void OnPause()
    {
        if (pauseBehavs != null)
        {
            return;
        }

        // �L���ȃR���|�[�l���g���擾
        pauseBehavs = Array.FindAll(GetComponentsInChildren<Behaviour>(), (obj) => { return obj.enabled; });
        foreach (var com in pauseBehavs)
        {
            com.enabled = false;
        }

        rgBodies = Array.FindAll(GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
        rgBodyVels = new Vector3[rgBodies.Length];
        rgBodyAVels = new Vector3[rgBodies.Length];
        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodyVels[i] = rgBodies[i].velocity;
            rgBodyAVels[i] = rgBodies[i].angularVelocity;
            rgBodies[i].Sleep();
        }


    }

    // �|�[�Y�������ꂽ�Ƃ�
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // �|�[�Y�O�̏�ԂɃR���|�[�l���g�̗L����Ԃ𕜌�
        foreach (var com in pauseBehavs)
        {
            com.enabled = true;
        }

        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodies[i].WakeUp();
            rgBodies[i].velocity = rgBodyVels[i];
            rgBodies[i].angularVelocity = rgBodyAVels[i];
        }



        pauseBehavs = null;

        rgBodies = null;
        rgBodyVels = null;
        rgBodyAVels = null;


    }

    // �|�[�Y
    public static void pause()
    {
        foreach (var obj in targets)
        {
            obj.OnPause();
        }
    }

    // �|�[�Y����
    public static void Resume()
    {
        foreach (var obj in targets)
        {
            obj.OnResume();
        }
    }

    public static void DestoryObj()
    {
        List<Pause> list = new List<Pause>(targets);

        foreach (var obj in list)
        {
            obj.OnDestory();
        }
    }
}
