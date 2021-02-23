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
            StartCoroutine(battle.PlayerAttack(cardDisplay.card.name, 10));
            Destroy(draggable.gameObject);
            Destroy(draggable.placeholder);
        }
        else if (draggable != null)
        {
            draggable.home = transform;
        }
    }

}
