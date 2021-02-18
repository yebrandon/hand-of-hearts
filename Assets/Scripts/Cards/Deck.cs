﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<string> cards = new List<string>(){
        "Strike",
        "Guard",
        "Recover"
    };
    public DropZone hand;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log('a');
        for (int i = 0; i < 3; i++)
        {
            Debug.Log('b');
            Draw();
        }
    }

    public void Draw()
    {
        Debug.Log('c');
        GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + cards[Random.Range(0, cards.Count)]));

        card.transform.SetParent(hand.transform);
        card.GetComponent<CardDisplay>().home = hand.transform;
        card.transform.localScale = new Vector3(1, 1, 1);
    }
}
