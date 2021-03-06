using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NextSceneAdd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
    }

    public void RestartBattle()
    {
        SceneManager.LoadScene(SceneTracker.lastBattleSceneName);
    }

    public void ReturnToTitle()
    {
        Relationships.ResetRelationships();
        SceneManager.LoadScene("Start");
    }
}
