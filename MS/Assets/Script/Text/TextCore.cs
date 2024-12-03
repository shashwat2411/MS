using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//会話内容を保持するための関数
[System.Serializable]
public class TextCore
{
    public int number;

    [Tooltip("喋っている対象")]
    public string _talkName = "";

    [Tooltip("喋っている対象の画像")]
    public Sprite _talkCharaImage = null;

    [Tooltip("喋っている内容")]
    public string _talkInfo = "";

    [Tooltip("選択肢ありかなしか")]
    public bool select = false;

    [Tooltip("選択肢のテキスト")]
    public string[] choices;

    [Tooltip("選択肢ごとの次のテキストインデックス")]
    public int[] nextIndexes;

}
