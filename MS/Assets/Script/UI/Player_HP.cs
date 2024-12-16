using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP : MonoBehaviour
{
    public float Hp_Now;
    public float Hp_Max;
    public float shiftSpeed = 0.3f;

    [SerializeField]
    Image shiftBar, baseBar;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        Hp_Max = 100.0f;
        Hp_Now = 100.0f;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (active == true) 
        {
            baseBar.fillAmount = Hp_Now / Hp_Max;
            shiftBar.fillAmount = Mathf.Lerp(shiftBar.fillAmount, baseBar.fillAmount, shiftSpeed);
        }
    }
}
