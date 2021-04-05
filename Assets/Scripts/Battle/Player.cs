using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Duelist
{
    public Deck deck;
    public Relationship relationship;

    void Awake()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        relationship = GameObject.Find("RelationshipStatus").GetComponent<Relationship>();
    }
}
