using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CandyChoice1 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "oOh Christmas.... it's been so long since I PROPERLY celebrated, candy canes are too spicy for my liking though...";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "mmm Easter is nice... the children always go about LOSING my candy though... no chance to ENJOY it.";
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "YOU RIGHT. I can never pass up a chance to give SWEETS and Valentine's was ALWAYS when my shop was BOOMING!";
        ChoiceMade = 3;
        Relationships.relationships["Candy"]++;
        Relationship.change = true;
    }

    public static int getChoice()
    {
        return ChoiceMade;
    }

    public static void setChoice(int num)
    {
        ChoiceMade = num;
    }

    void Start()
    {
        continueBtn.SetActive(false);
    }

    void Update()
    {
        if (ChoiceMade >= 1)
        {
            Choice01.SetActive(false);
            Choice02.SetActive(false);
            Choice03.SetActive(false);
            continueBtn.SetActive(true);
        }
    }
}
