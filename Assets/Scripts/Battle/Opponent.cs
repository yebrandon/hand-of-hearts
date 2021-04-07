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
            foreach (var x in hand)
            {
                Debug.Log(x.ToString());
            };

            if (mana >= 2)
            {
                if (hand.Contains("Sugar") && HP < 60 && lastPlayedCardName != "Sugar")
                {
                    cardToPlayName = "Sugar";
                    hand.Remove("Sugar");
                }
                else if (mana >= 3)
                {
                    int draw = Random.Range(0, 5);
                    if (draw == 0 && !hand.Contains("Exchangemint"))
                    {
                        return "Draw";
                    }
                    else
                    {
                        if (mana >= 10 && hand.Contains("Exchangemint"))
                        {
                            cardToPlayName = "Exchangemint";
                            hand.Remove("Exchangemint");
                        }
                        else if (mana >= 7 && hand.Contains("Sticky") && draw < 3)
                        {
                            cardToPlayName = "Sticky";
                            hand.Remove("Sticky");
                        }
                        else if (mana >= 6 && hand.Contains("Candied") && draw < 2)
                        {
                            cardToPlayName = "Candied";
                            hand.Remove("Candied");
                        }
                        else if (mana >= 5 && hand.Contains("Toothache") && draw < 2)
                        {
                            cardToPlayName = "Toothache";
                            hand.Remove("Toothache");
                        }
                        else if (mana >= 4 && hand.Contains("Sucker"))
                        {
                            cardToPlayName = "Sucker";
                            hand.Remove("Sucker");
                        }
                        else
                        {
                            return "EndTurn";
                        }
                    }
                }
                else
                {
                    return "EndTurn";
                }
            }
            else
            {
                return "EndTurn";
            }
        }
        else if (charName == "Jibb")
        {
            if (battle.sneak && hand.Contains("Quick Swipe") && mana >= 2)
            {
                cardToPlayName = "Quick Swipe";
                hand.Remove("Quick Swipe");
            }
            else if (mana >= 9)
            {
                return "Draw";
            }
            else if (mana >= 8 && hand.Contains("Sneak Attack"))
            {
                cardToPlayName = "Sneak Attack";
                hand.Remove("Sneak Attack");
            }
            else if (mana >= 6 && hand.Contains("Boid"))
            {
                cardToPlayName = "Boid";
                hand.Remove("Boid");
            }
            else if (mana >= 5 && hand.Contains("Cloak"))
            {
                cardToPlayName = "Cloak";
                hand.Remove("Cloak");
            }
            else if (mana >= 2 && hand.Contains("Quick Swipe"))
            {
                cardToPlayName = "Quick Swipe";
                hand.Remove("Quick Swipe");
            }
            else if (mana >= 1 && hand.Contains("Body Substitutes") && shield <= 40)
            {
                cardToPlayName = "Body Substitutes";
                hand.Remove("Body Substitutes");
            }
            else if (mana >= 1 && hand.Contains("Stolen Potion") && HP <= 90)
            {
                cardToPlayName = "Stolen Potion";
                hand.Remove("Stolen Potion");
            }
            else
            {
                return "EndTurn";
            }
        }
        else if (charName == "Rosa")
        {
            if (mana >= 9)
            {
                return "Draw";
            }
            else if (mana >= 3 && shield >= 50 && hand.Contains("Garden of None"))
            {
                cardToPlayName = "Garden of None";
                hand.Remove("Garden of None");
            }
            else if (mana >= 7 && hand.Contains("Fate's Wreath") && shield <= 50)
            {
                cardToPlayName = "Fate's Wreath";
                hand.Remove("Fate's Wreath");
            }
            else if (mana >= 3 && hand.Contains("Veil of Thorns")) // and not thorns?
            {
                cardToPlayName = "Veil of Thorns";
                hand.Remove("Veil of Thorns");
            }
            else if (mana >= 3 && shield >= 20 && hand.Contains("Garden of None"))
            {
                cardToPlayName = "Garden of None";
                hand.Remove("Garden of None");
            }
            else if (mana >= 1 && HP >= 20 && shield <= 90 && hand.Contains("Lament"))
            {
                cardToPlayName = "Lament";
                hand.Remove("Lament");
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
