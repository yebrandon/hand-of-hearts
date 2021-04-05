using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public GameObject gameOverScreen;
    public GameObject cardArea;
    public GameObject cardToSpawn;

    public Text relationshipText;
    public Text cardText;
    public Text descriptionText;
    
    
    public string wonCardFile;
    public string description;

    // if you win
    public void LoadCard(Relationship relationshipStatus, string opponentName, Deck playerDeck)
    {
        Debug.Log("hello");

        // set relationship status
        relationshipText.text = relationshipStatus.getStatus() + " with " + opponentName;
        
        // set new card
        
        wonCardFile = opponentName + "/SkillCard" + (int)(relationshipStatus.getStatusInt() + 1);
        Debug.Log(wonCardFile);
        // load card prefab
        GameObject cardPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/SkillCards/" + wonCardFile));
        cardPrefab.transform.SetParent(cardArea.transform);
        cardPrefab.transform.localPosition = new Vector3(1, 1, 1);
        cardPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // // add card to player's hand
        // playerDeck.AddCard(wonCardFile);
        // foreach( var x in playerDeck.cards) {
        //     Debug.Log( x.ToString());
        // };

        // display card info
        cardText.text = cardPrefab.GetComponent<CardDisplay>().card.name;
        description = "You've won a card from " + opponentName + "! This is what your new card can do: \n";
        descriptionText.text = description + cardPrefab.GetComponent<CardDisplay>().card.description;

        
    }

    public void OnContinueButton()
    {
        Debug.Log("continue pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
    }

}