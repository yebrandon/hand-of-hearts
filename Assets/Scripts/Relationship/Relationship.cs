using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum RelationshipStatus { NOTMET, STRANGERS, ACQUAINTANCES, FRIENDS, HEARTWON }
public class Relationship : MonoBehaviour
{
    public Opponent opponent;
    public Text dialogueText;
    public int current = 0;
    public Dictionary<int, RelationshipStatus> status = new Dictionary<int, RelationshipStatus>{
        {0, RelationshipStatus.STRANGERS},
        {1, RelationshipStatus.ACQUAINTANCES},
        {2, RelationshipStatus.FRIENDS},
        {3, RelationshipStatus.HEARTWON}
    };

    void Start()
    {
        current = 0;
        dialogueText.text = status[current].ToString();
    }

    public RelationshipStatus getStatus()
    {
        return status[current];
    }

    public void setStatus(String charName)
    {
        dialogueText.text = Relationships.relationships[charName].ToString();
    }
}
