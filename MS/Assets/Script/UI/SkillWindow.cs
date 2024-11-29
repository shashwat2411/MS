using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillWindow : MonoBehaviour
{

    public BonusData Bonus;

    [SerializeField]
    Image Icon;

    [SerializeField]
    TextMeshProUGUI Name, Description;


  
    // Start is called before the first frame update
    void Start()
    {
        
    }

   


    public void DrawBonus()
    {
        

        Name.text = Bonus.name;
        Description.text = Bonus.description;
        Icon.sprite = Bonus.icon;
    }

  
   
}
