using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<string> cards;
    public DropZone hand;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize list of cards
        cards = new List<string>()
        {
            "Strike",
            "Strike",
            "Strike",
            "Guard",
            "Recover",
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
        GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + cards[Random.Range(0, cards.Count)]));
        card.transform.SetParent(hand.transform);
        card.GetComponent<Draggable>().home = hand.transform;
        card.transform.localScale = new Vector3(1, 1, 1);
    }
}
