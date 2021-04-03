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
        {0, RelationshipStatus.NOTMET},
        {1, RelationshipStatus.STRANGERS},
        {2, RelationshipStatus.ACQUAINTANCES},
        {3, RelationshipStatus.FRIENDS},
        {4, RelationshipStatus.HEARTWON}
    };

    void Start()
    {
        current = 1;
        dialogueText.text = status[current].ToString();
    }

    public RelationshipStatus getStatus()
    {
        return status[current];
    }

    public void setStatus(String charName)
    {
        Debug.Log(Relationships.relationships[charName].ToString());
        dialogueText.text = Relationships.relationships[charName].ToString();
    }
}
