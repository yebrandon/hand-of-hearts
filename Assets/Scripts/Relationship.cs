using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum RelationshipStatus { STRANGERS, ACQUAINTANCES, FRIENDS, HEARTWON }
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

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        dialogueText.text = status[current].ToString();
    }

    public RelationshipStatus getStatus()
    {
        return status[current];
    }

    public int getStatusInt()
    {
        return current;
    }

    public void setStatus(String charName)
    {
        Debug.Log("setting status");
        dialogueText.text = Relationships.relationships[charName].ToString();
    }

    /*     public void setStatus(int choice)
        {
            if (choice == 1 && current < 2)
            {
                current++;
            }

            Debug.Log("setting status");
            Debug.Log(current);
            Debug.Log(status[current].ToString());
            dialogueText.text = status[current].ToString();

            // set relationship status in data type thing that stays what is comments
            Relationships.relationships[opponent.charName] = getStatus();
        } */

    // Update is called once per frame
    void Update()
    {

    }
}
