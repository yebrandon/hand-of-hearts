using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChoice : MonoBehaviour
{
    void Start()
    {
        // Loop through relationships and enable character choices in the scene if their heart has been won
        foreach (KeyValuePair<string, RelationshipStatus> entry in Relationships.relationships)
        {
            if (entry.Value == RelationshipStatus.HEARTWON)
            {
                this.transform.Find(entry.Key).gameObject.SetActive(true);
            }
        }
    }

    // Load the ending corresponding to the user's choice. Remember to pass in the correct number in the inspector for the button that calls this function
    public void Choose(int num)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + num);
    }


}
