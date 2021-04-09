using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public DropZone menu;
    public Battle battle;

    // Restock cards in the menu
    public void updateCards()
    {
        foreach (Transform child in menu.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, RelationshipStatus> entry in Relationships.relationships)
        {
            // If entry is not for our current opponent or Blossom and is a higher status than notmet
            if (entry.Key != battle.opponentUnit.charName && entry.Key != "Blossom" && entry.Value > 0)
            {
                // Create card based on relationship status 
                GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/SkillCards/" + entry.Key + "/SkillCard" + ((int)entry.Value).ToString()));

                // Attach card to menu
                card.transform.SetParent(menu.transform);
                card.GetComponent<Draggable>().home = menu.transform;
                card.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void toggleMenu()
    {

        menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);

    }
}
