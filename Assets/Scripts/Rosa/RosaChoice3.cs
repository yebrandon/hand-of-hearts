using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RosaChoice3 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "As much as it hurts to admit it… you might be right…";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "You really believe that? It brings me a bit of hope knowing that someone believes in me…";
        ChoiceMade = 2;
        Relationships.relationships["Rosa"]++;
        Relationship.change = true;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "I suppose fixing everything now might be an impossible goal… but still…";
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
        ChoiceMade = 0;
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
