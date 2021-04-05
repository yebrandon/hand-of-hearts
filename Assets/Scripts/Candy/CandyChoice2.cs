using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CandyChoice2 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "MM YUM!! You can NEVER run out of the flavour because of the endless CHEW. Maybe I'll give you bite of my fresh toffee to try...";
        ChoiceMade = 1;
        Relationships.relationships["Candy"]++;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "mm I do love the CRONCH but I prefer to SAVOUR my hard candies and suck on them.";
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "yum chocolate is nice... she doesn't have enough CHEW for my taste though.";
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
