using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatedBy : MonoBehaviour
{
    public Text defeatText;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(SceneTracker.lastBattleCharName);
        defeatText.text = "You were defeated by \n" + SceneTracker.lastBattleCharName + "!";
    }
}
