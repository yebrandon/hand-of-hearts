using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum RelationshipStatus { NOTMET, STRANGERS, ACQUAINTANCES, FRIENDS, HEARTWON }
public class Relationship : MonoBehaviour
{
    public Opponent opponent;
    // public Text dialogueText;
    public int current = 0;
    public GameObject heartsGroup;
    // public GameObject [] newHeart = new GameObject[3];
    public Dictionary<int, RelationshipStatus> status = new Dictionary<int, RelationshipStatus>{
        {0, RelationshipStatus.STRANGERS},
        {1, RelationshipStatus.ACQUAINTANCES},
        {2, RelationshipStatus.FRIENDS},
        {3, RelationshipStatus.HEARTWON}
    };
    public static bool change = false;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;

        // dialogueText.text = status[current].ToString();
    }

    public RelationshipStatus getStatus()
    {
        Debug.Log(status[current]);
        return status[current];
    }

    public int getStatusInt()
    {
        return current;
    }

    public void setStatus(String charName, bool start = false)
    {
        Debug.Log("setting status");
        Debug.Log(Relationships.relationships[charName]);
        if (!start)
        {
            current++;
            Debug.Log(current);
            if (change)
            {
                setHearts();
                change = false;
            }

        }
        // dialogueText.text = Relationships.relationships[charName].ToString();
    }

    public void setHearts()
    {
        SpriteRenderer oldHeart = heartsGroup.GetComponentInChildren<SpriteRenderer>();
        GameObject newHeart = (GameObject)Instantiate(Resources.Load("Prefabs/Hearts/heartFilledSparkle"));
        newHeart.transform.SetParent(heartsGroup.transform);
        // Debug.Log(oldHeart.transform.localPosition);
        newHeart.transform.localPosition = (oldHeart.transform.localPosition);
        newHeart.transform.localScale = new Vector3(14f, 14f, 14f);
        Destroy(oldHeart);


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