using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }


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

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            if (SceneManager.sceneCount == 1)
            {
                EndDialogue();
                return;
            }
            else
            {
                FindObjectOfType<SwitchScene>().NextSceneAdd();
            }
        }


        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        FindObjectOfType<SwitchScene>().NextScene();
    }
}
