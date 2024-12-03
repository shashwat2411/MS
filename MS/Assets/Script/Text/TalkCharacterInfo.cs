using UnityEngine;

public class TalkCharacterInfo : MonoBehaviour
{
    public TextCore[] talkText;

    private void Start()
    {
        
    }

    void Init()
    {
        for(int i = 0; i < talkText.Length - 1; i++)
        {
            talkText[0].number = i;
        }
    }

    public TextCore GetNowText(int i) { return talkText[i]; }
    public int GetTextLangth() { return talkText.Length; }
}

