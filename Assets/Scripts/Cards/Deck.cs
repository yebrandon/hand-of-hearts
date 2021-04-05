using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<string> cards;
    public DropZone hand;
    public string toDraw;
    public int spawnedTalks = 0;
    public const int MAX_HAND_SIZE = 6;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize list of cards
        cards = new List<string>()
        {
            "Strike",
            "Strike",
            "Guard",
            "Recover",
            "Talk",
            "Talk",
            "Cross",
            "Burn",
            "Edge",
            "Edge",
        };

        // Draw 3 cards at the start of the game (+1 for start of turn)
        for (int i = 0; i < 3; i++)
        {
            Draw();
        }
    }

    public bool checkThird(string toDraw)
    {
        int repeat = 0;
        foreach (Transform child in hand.transform)
        {
            if ((toDraw+"(Clone)").Equals(child.name)){
                repeat++;
            }
            if (repeat == 2){
                return true;
            }
        }
        return false;
    }

    public void Draw()
    {
        if (hand.transform.childCount < MAX_HAND_SIZE)
        {
            toDraw = cards[Random.Range(0, cards.Count)];
            while ((toDraw == "Talk" && spawnedTalks >= 3) || checkThird(toDraw))
            {
                toDraw = cards[Random.Range(0, cards.Count)];
            }
            if (toDraw == "Talk")
            {
                spawnedTalks++;
            }
            Debug.Log("spawned" + spawnedTalks);

            GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + toDraw));
            Debug.Log(toDraw);
            card.transform.SetParent(hand.transform);
            card.GetComponent<Draggable>().home = hand.transform;
            card.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
