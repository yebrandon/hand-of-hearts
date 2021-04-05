using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CandyChoice3 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "AGREED. Whisks are the WORST, especially to clean and they don't even WORK that well. Hrmm maybe I just have to invent my own replacement...";
        ChoiceMade = 1;
        Relationships.relationships["Candy"]++;
        Relationship.change = true;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "Hrm I could imagine... I don't REALLY grate cheese very often so I wouldn't really know.";
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "Just keep em on the ring they come with!!! It's not THAT hard, organization is the MOST important part of the kitchen.";
        ChoiceMade = 3;
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
