using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDialogue : MonoBehaviour
{
    private int currentIndex;
    private int nextIndex;
    public Animator[] dialogueBox = new Animator[3];

    private void Start()
    {
        currentIndex = 0;
        nextIndex = 0;

        dialogueBox[0] = transform.GetChild(0).GetComponentInChildren<Animator>();
        dialogueBox[1] = transform.GetChild(1).GetComponentInChildren<Animator>();
        dialogueBox[2] = transform.GetChild(2).GetComponentInChildren<Animator>();

        dialogueBox[0].Play("DialogueBoxDisappearIdle");
        dialogueBox[1].Play("DialogueBoxDisappearIdle");
        dialogueBox[2].Play("DialogueBoxDisappearIdle");
    }

    private void CalculateNextBox()
    {
        while(nextIndex == currentIndex)
        {
            nextIndex = Random.Range(0, 3);
        }

        currentIndex = nextIndex;
    }

    public void ActivateDialogue(bool appear = true)
    {
        CalculateNextBox();

        if (dialogueBox[currentIndex] != null)
        {
            if (dialogueBox[currentIndex].GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                if (appear == true)
                {
                    dialogueBox[currentIndex].Play("DialogueBoxAppear");
                }
                else
                {
                    dialogueBox[currentIndex].Play("DialogueBoxDisappearIdle");
                }
            }
        }
    }
}
