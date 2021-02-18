﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (tag == "PlayArea")
        {
            Debug.Log(eventData.pointerDrag.name + " was played!");
            Destroy(cardDisplay.gameObject);
        }
        else if (cardDisplay != null && tag == "PlayArea")
        {
            cardDisplay.home = transform;

        }
    }

}
