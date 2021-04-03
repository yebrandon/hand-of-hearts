using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChoice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (KeyValuePair<string, RelationshipStatus> entry in Relationships.relationships)
        {
            if (entry.Value == RelationshipStatus.HEARTWON)
            {
                this.transform.Find(entry.Key).gameObject.SetActive(true);
            }
        }
    }

    // Loads the ending corresponding to the user's choice. Remember to pass in the correct number in the inspector for the button that calls this function
    public void Choose(int num)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + num);
    }


}
