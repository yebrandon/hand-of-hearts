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
                if (hand.Contains("Chaos") && numButterfliesPlayed > 0)
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
            else if (mana >= 3 && hand.Contains("Blue Morpho Butterfly"))
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
        else if (charName == "Candy")
        {
            Debug.Log("opponent turn");
            foreach( var x in hand) {
                Debug.Log( x.ToString());
            };
            
            if (mana >= 2)
            {
                if (hand.Contains("Sugar") && HP < 60)
                {
                    cardToPlayName = "Sugar";
                    hand.Remove("Sugar");
                }
                else if (mana >= 3)
                {
                    int draw = Random.Range(0, 5);
                    if (draw == 0)
                    {
                        return "Draw";
                    }
                    else
                    {
                        if (mana >= 4 && hand.Contains("pog"))
                        {
                            cardToPlayName = "pog";
                            hand.Remove("pog");
                        }
                        else if (mana >= 5 && hand.Contains("Toothache"))
                        {
                            cardToPlayName = "Toothache";
                            hand.Remove("Toothache");
                        }
                        else if (mana >= 6 && hand.Contains("Candied"))
                        {
                            cardToPlayName = "Candied";
                            hand.Remove("Candied");
                        }
                        else if (mana >= 7 && hand.Contains("Sticky"))
                        {
                            cardToPlayName = "Sticky";
                            hand.Remove("Sticky");
                        }
                        else if (mana >= 10 && hand.Contains("Exchangemint"))
                        {
                            cardToPlayName = "Exchangemint";
                            hand.Remove("Exchangemint");
                        }
                    }
                }
                else {
                    return "EndTurn";
                }
            }
            else
            {
                return "EndTurn";
            }

        }
        Debug.Log(charName);
        Debug.Log(cardToPlayName);
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
