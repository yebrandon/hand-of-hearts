using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform home;
    public Transform placeholderHome;
    public GameObject placeholder;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create placeholder
        placeholder = new GameObject();
        placeholder.transform.SetParent(transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        // Set parents
        home = transform.parent;
        placeholderHome = home;
        transform.SetParent(transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Make card follow cursor

        // Place placeholder correctly
        if (placeholder.transform.parent != placeholderHome)
        {
            placeholder.transform.SetParent(placeholderHome);
        }

        int newSiblingIndex = home.childCount;

        for (int i = 0; i < home.childCount; i++)
        {

            if (transform.position.x < home.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Set card back to correct position
        transform.SetParent(home);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }

}
