using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToBattle : MonoBehaviour
{
    public TalkChoice talkChoice;

    // Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Talk"));
    }

    public void UnloadScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BattleDemo"));
        SceneManager.UnloadSceneAsync("Talk");
    }
}
