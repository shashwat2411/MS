using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillWindow : MonoBehaviour
{

    BonusData Bonus;

    [SerializeField]
    Image Icon;

    [SerializeField]
    TextMeshProUGUI Name, Description;
    // Start is called before the first frame update
    void Start()
    {
        Bonus = BonusSettings.Instance.bonusDatas[0];

        DrawSkill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawSkill()
    {
        Name.text = Bonus.name;
        Description.text = Bonus.description;
        Icon.sprite = Bonus.icon;
    }
}
