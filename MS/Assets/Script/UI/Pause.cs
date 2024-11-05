using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pause : MonoBehaviour
{
    static List<Pause> targets = new List<Pause>();   // ポーズ対象のスクリプト

    // ポーズ対象のコンポーネント
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



        // ポーズ対象に追加する
        targets.Add(this);
    }


    // 破棄されるとき
    void OnDestory()
    {
        // ポーズ対象から除外する
        targets.Remove(this);
    }

    // ポーズされたとき
    void OnPause()
    {
        if (pauseBehavs != null)
        {
            return;
        }

        // 有効なコンポーネントを取得
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

    // ポーズ解除されたとき
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // ポーズ前の状態にコンポーネントの有効状態を復元
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

    // ポーズ
    public static void pause()
    {
        foreach (var obj in targets)
        {
            obj.OnPause();
        }
    }

    // ポーズ解除
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
