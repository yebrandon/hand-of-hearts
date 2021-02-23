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
            draggable.placeholderHome = transform;
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
            draggable.placeholderHome = draggable.home;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (tag == "PlayArea")
        {
            Debug.Log(eventData.pointerDrag.name + " was played!");
            Debug.Log(cardDisplay.card.name);

            // Play the card

            if (cardDisplay.card.name == "Strike")
            {
                Debug.Log('c');
                StartCoroutine(battle.PlayerAttack(10));
            }
            else if (cardDisplay.card.name == "Guard")
            {
                StartCoroutine(battle.PlayerDefend(10));
            }
            else if (cardDisplay.card.name == "Recover")
            {
                Debug.Log('c');
                StartCoroutine(battle.PlayerHeal(5));
            }

            Destroy(draggable.gameObject);
            Destroy(draggable.placeholder);
        }
        else if (draggable != null)
        {
            draggable.home = transform;
        }
    }

}
