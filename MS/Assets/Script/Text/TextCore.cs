using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//会話内容を保持するための関数
[System.Serializable]
public class TextCore
{

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

    [Tooltip("番号を飛ばすかいなか")]
    public bool isCheck = false;

    [Tooltip("飛ばすなら何処へ")]
    public int changeNumber;

}
