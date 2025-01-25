//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TutorialManager : MonoBehaviour
//{
//    public TutorialMonitorManager video;
//    public TutorialTextManager text;

//    public enum TUTORIALSTATE
//    { 
//        MOVEMENT = 0,
//        CHARGE,
//        EXPERIENCE,
//        LEVEL_UP,
//        CHARGE_LEVEL,
//        AIMER_MOVEMENT,
//        DASH,
//        MP,
//        FINISH,
        
//        MAX
//    }

//    public TUTORIALSTATE state;
        
//    private void Start()
//    {
//        state = TUTORIALSTATE.MOVEMENT;
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Z))
//        {
//            NextState();
//        }
//    }

//    private void NextState()
//    {
//        int current = (int)state;
        
//        current++;
//        if (current >= (int)TUTORIALSTATE.MAX) { current = (int)TUTORIALSTATE.FINISH; }

//        video.ChangeScreen();
//        text.ChangeScreen();

//        state = (TUTORIALSTATE)current;
//    }
//}
