using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToBattle : MonoBehaviour
{

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
        SceneManager.SetActiveScene(gameObject.scene);
    }

    public void UnloadScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex - Battle.talksPlayed * 2));
        Debug.Log(SceneManager.GetActiveScene().name);

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(gameObject.scene.buildIndex - 1));
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
