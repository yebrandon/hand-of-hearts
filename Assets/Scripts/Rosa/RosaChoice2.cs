using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RosaChoice2 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "Oh-, okay, forget I said anything then…";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "No, the thought just simply crossed my mind…";
        ChoiceMade = 2;

    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "So I’m not the only one that feels like this… In a way… it makes me feel a bit less alone…";
        ChoiceMade = 3;
        Relationships.relationships["Rosa"]++;
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
