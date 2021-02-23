using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Battle battle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();

        if (cardDisplay != null)
        {
            cardDisplay.placeholderHome = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();

        if (cardDisplay != null && cardDisplay.placeholderHome == transform)
        {
            cardDisplay.placeholderHome = cardDisplay.home;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (tag == "PlayArea")
        {
            Debug.Log(eventData.pointerDrag.name + " was played!");
            Debug.Log(cardDisplay.card.name);

            // Play the card

            StartCoroutine(battle.PlayerAttack(cardDisplay.card.name, 10));
            
            Destroy(cardDisplay.gameObject);
        }
        else if (cardDisplay != null)
        {
            cardDisplay.home = transform;
        }
    }

}
