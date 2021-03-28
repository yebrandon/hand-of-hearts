using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Image artworkImage;

    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();
        artworkImage.sprite = card.artwork;
    }

    void Update()
    {
        // Prevent player from playing cards if it is not their turn
        if (FindObjectOfType<Battle>().state != BattleState.PLAYERTURN)
        {
            GetComponent<Draggable>().enabled = false;
        }
        else
        {
            GetComponent<Draggable>().enabled = true;
        }
    }
}
