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
        foreach (KeyValuePair<string, RelationshipStatus> entry in Relationships.relationships)
        {
            // If entry is not for our current opponent and is a higher status than strangers
            if (entry.Key != battle.opponentUnit.charName && (int)entry.Value > 1)
            {
                // Create card based on relationship status 
                // SkillCard1 maps to acquaintences, SkillCard2 maps to friends, etc.
                GameObject card = (GameObject)Instantiate(Resources.Load("Prefabs/SkillCards/" + battle.opponentUnit.charName + "/SkillCard" + ((int)entry.Value).ToString()));

                // Attach card to menu
                card.transform.SetParent(menu.transform);
                card.GetComponent<Draggable>().home = menu.transform;
                card.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void toggleMenu()
    {
        if (battle.state == BattleState.PLAYERTURN)
        {
            menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
        }
    }
}
