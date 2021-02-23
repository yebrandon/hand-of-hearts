using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Duelist
{
    public List<string> cards = new List<string>() { };
    public DropZone cardZone;

    public void Play()
    {
        GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + cards[Random.Range(0, cards.Count)]));
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
