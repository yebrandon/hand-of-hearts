using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToBattle : MonoBehaviour
{
    public void UnloadScene()
    {
        //BattleDemo =
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BattleDemo"));
        SceneManager.UnloadSceneAsync("Talk");
        //StartCoroutine(OpponentAttack()); // start opponent attack coroutine
    }
}
