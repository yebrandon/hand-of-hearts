using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConstantsChoice2 : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade;
    public GameObject continueBtn;


    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "oh dear, the traditional math curriculum they teach in schools is soo not magical...";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "ah, another wandering duck! how glad i am to have found another!";
        ChoiceMade = 2;
        Relationships.relationships["Constants"]++;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "curly fries... far too complicated for my taste. i much prefer the organization of french fries!";
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
