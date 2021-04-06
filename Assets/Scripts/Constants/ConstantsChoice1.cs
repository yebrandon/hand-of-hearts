using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConstantsChoice1 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;


    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "o-oh! um maybe... i do really like french fries...";
        ChoiceMade = 1;
        Relationships.relationships["Constants"]++;
        Relationship.change = true;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "maybe... but i first and foremost i want my french fries back!";
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "sounds like it...";
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
