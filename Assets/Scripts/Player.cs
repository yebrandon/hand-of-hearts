using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Duelist
{
    public Deck deck;

    void Awake()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
    }
}
