using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum RelationshipStatus { ENEMIES, PALS, FRIENDS, CHUMS, LOVERS }
public class Relationship : MonoBehaviour
{
    public Text dialogueText;
    public int current = 0;
    public Dictionary<int, RelationshipStatus> status = new Dictionary<int,RelationshipStatus>{
        {0, RelationshipStatus.ENEMIES},
        {1, RelationshipStatus.PALS},
        {2, RelationshipStatus.FRIENDS}
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

    public void setStatus(int choice)
    {
        if (choice == 1 && current < 2)
        {
            current++; 
        }

        Debug.Log("setting status");
        Debug.Log(current);
        Debug.Log(status[current].ToString());
        dialogueText.text = status[current].ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
