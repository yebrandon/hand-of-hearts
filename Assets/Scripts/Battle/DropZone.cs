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

        // Check if player has enough mana to play card
        if (tag == "PlayArea" && cardDisplay.card.cost <= battle.playerUnit.mana)
        {
            // Play card
            StartCoroutine(battle.PlayerAttack(cardDisplay.card));
            Destroy(draggable.gameObject);
            Destroy(draggable.placeholder);
        }
    }

}
