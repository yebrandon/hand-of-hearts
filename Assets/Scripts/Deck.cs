using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<string> cards = new List<string>(){
        "Strike",
        "Guard",
        "Recover"
    };
    public DropZone hand;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++){
            GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/" + cards[Random.Range(0, cards.Count)]));
            card.transform.SetParent(hand.transform);
            card.GetComponent<CardDisplay>().home = hand.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
