using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
    public bool AnimeStart, AnimeEnd;

    Animator AnimeCon;

    // Start is called before the first frame update
    void Start()
    {
        AnimeStart = false;
        AnimeEnd = false;

        AnimeCon = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimeCon.SetBool("PauseStart", AnimeStart);
        AnimeCon.SetBool("PauseEnd", AnimeEnd);
    }

    public void AnimeReset()
    {
        AnimeStart = false;
        AnimeEnd = false;
    }

    public void PauseAnimationStar()
    {
        AnimeStart = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PauseAnimationEnd()
    {
        AnimeEnd = true;
    }
}
