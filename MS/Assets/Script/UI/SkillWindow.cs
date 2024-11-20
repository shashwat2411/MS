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

        //DrawBonus();
    }

    public void RandomBonus()
    {
        int c = BonusSettings.Instance.playerBonusDatas.Count;

        int i = Random.Range(0, c);

        Bonus = BonusSettings.Instance.playerBonusDatas[i];
    }

    

    public void DrawBonus()
    {
        RandomBonus();

        Name.text = Bonus.name;
        Description.text = Bonus.description;
        Icon.sprite = Bonus.icon;
    }

   
}
