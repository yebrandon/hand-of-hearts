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

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggable != null)
        {
            draggable.placeholderHome = transform; // Create placeholder
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggable != null && draggable.placeholderHome == transform)
        {
            draggable.placeholderHome = draggable.home; // Remove placeholder
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (tag == "PlayArea")
        {
            // Play card
            StartCoroutine(battle.PlayerAttack(cardDisplay.card, 10));
            Destroy(draggable.gameObject);
            Destroy(draggable.placeholder);
        }
        else if (draggable != null)
        {
            draggable.home = transform; // Return card to hand
        }
    }

}
