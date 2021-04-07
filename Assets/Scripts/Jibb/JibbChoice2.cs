using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JibbChoice2 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "Even so, I would never subject myself to thaaat. Risking my life for their sake after they took everything from me… Ha! not a chaaaance!";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "What’s with the dramaaaatics? It sounds like you just walked out of a theatre…Buuuut, I get what you’re saying. People like us gotta do whatever it takes to stay alive.";
        ChoiceMade = 2;
        Relationships.relationships["Jibb"]++;
        Relationship.change = true;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "Absoluuuutely not. Giving anything of value to the capital after they took everything from me: the notion in itself disguuuusts me.";
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
