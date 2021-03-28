﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<string> cards;
    public DropZone hand;
    public string toDraw;
    public int spawnedTalks = 0;
    public const int MAX_HAND_SIZE = 6;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize list of cards
        cards = new List<string>()
        {
            "Strike",
            "Strike",
            "Guard",
            "Recover",
            "Talk",
            "Talk"
        };

        // Draw 3 cards at the start of the game (+1 for start of turn)
        for (int i = 0; i < 3; i++)
        {
            Draw();
        }
    }

    public void Draw()
    {
        if (hand.transform.childCount < MAX_HAND_SIZE)
        {
            toDraw = cards[Random.Range(0, cards.Count)];
            while (toDraw == "Talk" && spawnedTalks >= 3)
            {
                toDraw = cards[Random.Range(0, cards.Count)];
            }
            if (toDraw == "Talk")
            {
                spawnedTalks++;
            }
            Debug.Log("spawned" + spawnedTalks);

            GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + toDraw));
            card.transform.SetParent(hand.transform);
            card.GetComponent<Draggable>().home = hand.transform;
            card.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
