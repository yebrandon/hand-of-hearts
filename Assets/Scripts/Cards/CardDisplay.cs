using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Text nameText;
    public Text descriptionText;
    public Image artworkImage;

    // Use this for initialization
    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        artworkImage.sprite = card.artwork;
    }

    void Update()
    {
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
