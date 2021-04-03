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
        // Sets the active scene as the original battle scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex - Battle.talksPlayed * 2));

        // Unloads this scene as well as the scene before it
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(gameObject.scene.buildIndex - 1));
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
