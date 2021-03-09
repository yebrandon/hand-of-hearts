﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkChoice : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public static int ChoiceMade = 0;
    public GameObject continueBtn;

    public void ChoiceOption1()
    {
        TextBox.GetComponent<Text>().text = "I'm glad to hear that!";
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        TextBox.GetComponent<Text>().text = "That's unfortunate...";
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        TextBox.GetComponent<Text>().text = "Dam, u right";
        ChoiceMade = 3;
    }

    public static int getChoice()
    {
        return ChoiceMade;
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
