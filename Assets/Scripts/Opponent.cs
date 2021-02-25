using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Duelist
{
    public List<string> cards = new List<string>() { };
    public DropZone cardZone;
    public string playCard;

    public string chooseRandom()
    {
        return cards[Random.Range(0, cards.Count)];
    }

    public void Play()
    {
        playCard = chooseRandom();
        GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + playCard));
        Debug.Log("in opponent play");
        Debug.Log("card 1: " + playCard);
        Debug.Log("card 2: " + playCard);
        Debug.Log("card 3: " + playCard);
        card.transform.SetParent(cardZone.transform);
        card.GetComponent<Draggable>().home = cardZone.transform;
        card.transform.localScale = new Vector3(1, 1, 1);
    }

    public void EndTurn()
    {
        if (cardZone.transform.childCount == 1)
        {
            Destroy(cardZone.transform.GetChild(0).gameObject);
        }
    }
}
