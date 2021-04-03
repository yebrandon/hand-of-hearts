using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReward : MonoBehaviour
{
    void Start()
    {
        Debug.Log(SceneTracker.lastBattleCharName);
        if (Relationships.relationships[SceneTracker.lastBattleCharName] == RelationshipStatus.ACQUAINTANCES)
        {
            this.transform.Find("SkillCard1").gameObject.SetActive(true);
        }
        else if (Relationships.relationships[SceneTracker.lastBattleCharName] == RelationshipStatus.FRIENDS)
        {
            this.transform.Find("SkillCard2").gameObject.SetActive(true);
        }
        else if (Relationships.relationships[SceneTracker.lastBattleCharName] == RelationshipStatus.HEARTWON)
        {
            this.transform.Find("SkillCard3").gameObject.SetActive(true);
        }
    }
}
