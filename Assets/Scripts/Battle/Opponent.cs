using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Duelist
{
    public List<string> cards = new List<string>() { };
    public List<string> hand = new List<string>() { };
    public DropZone cardZone;
    public string cardToPlayName = "";
    public string lastPlayedCardName = "";
    public int numButterfliesPlayed = 0;
    public Battle battle;
    public CardDisplay cardDisplay;

    public int MAX_HAND_SIZE = 6;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Draw();
        }
    }

    public void Draw()
    {
        if (hand.Count < MAX_HAND_SIZE)
        {
            string toDraw = cards[Random.Range(0, cards.Count)];
            hand.Add(toDraw);
        }
    }

    public string ChooseAction()
    {
        if (charName == "Constants")
        {
            if (mana >= 10)
            {
                if (hand.Contains("Chaos"))
                {
                    cardToPlayName = "Chaos";
                    hand.Remove("Chaos");
                }
                else
                {
                    return "Draw";
                }
            }
            else if (mana >= 2 && lastPlayedCardName == "Blue Morpho Butterfly" && hand.Contains("Hofstadter's Butterfly"))
            {
                cardToPlayName = "Hofstadter's Butterfly";
                hand.Remove("Hofstadter's Butterfly");
                numButterfliesPlayed++;
            }
            else if (mana >= 2 && hand.Contains("Blue Morpho Butterfly"))
            {
                cardToPlayName = "Blue Morpho Butterfly";
                hand.Remove("Blue Morpho Butterfly");
                numButterfliesPlayed++;
            }
            else
            {
                return "EndTurn";
            }
        }

        GameObject cardGO = (GameObject)Instantiate(Resources.Load("Prefabs/" + charName + "Cards/" + cardToPlayName));

        cardDisplay = cardGO.GetComponent<CardDisplay>();

        // Attach card to cardzone
        cardGO.transform.SetParent(cardZone.transform);
        cardGO.GetComponent<Draggable>().home = cardZone.transform;
        cardGO.transform.localScale = new Vector3(1, 1, 1);

        return "Card";
    }

    public void clearCardZone()
    {
        Destroy(cardZone.transform.GetChild(0).gameObject);
    }

    public void EndTurn()
    {
        // Destroy card
        if (cardZone.transform.childCount == 1)
        {
            Destroy(cardZone.transform.GetChild(0).gameObject);
        }
    }
}
