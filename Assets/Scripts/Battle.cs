using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, OPPONENTTURN, WON, LOST }
public class Battle : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public GameObject endTurnButton;
    public GameOverUI gameOverUI;

    public Transform playerBattleStation;
    public Transform opponentBattleStation;

    public Player playerUnit;
    public Opponent opponentUnit;

    public BattleHUD playerHUD;
    public BattleHUD opponentHUD;

    public Text dialogueText;
    public Text turnText;
    public Text wonLost;

    public MenuButton menuButton;

    public int turnNum;

    public static int talksPlayed;
    public bool cross = false;
    public bool burn = false;
    public int burnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        turnNum = 0;
        talksPlayed = 0;
        state = BattleState.START; // set to the start state when the first frame is updated
        // set up the battle and start the coroutine
        gameOverUI.gameOverScreen.SetActive(false);
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }

    // sets up the battle by setting up the player and opponent GUI
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab); // instantiating the player's gameobject
        //playerGO.transform.SetParent(playerBattleStation);
        playerUnit = playerGO.GetComponent<Player>(); // gets the player's component

        // Change relationship status from NOTMET to strangers 
        Relationships.relationships[opponentUnit.charName]++;
        playerUnit.relationship.setStatus(opponentUnit.charName, true);

        turnText.text = "Your Turn";
        dialogueText.text = "The battle begins!"; // sets the dialogue text

        playerHUD.SetHUD(playerUnit); // setup player HUD
        opponentHUD.SetHUD(opponentUnit); // setup opponent HUD

        yield return new WaitForSeconds(2f); // waits for two seconds

        Debug.Log("player hp" + playerUnit.HP);
        Debug.Log("player shield" + playerUnit.shield);
        Debug.Log("opponent hp" + opponentUnit.HP);
        Debug.Log("opponent hp" + opponentUnit.shield);

        state = BattleState.PLAYERTURN; // change the battle state to be the player's turn
        PlayerTurn(); // run the player's turn function
    }

    // deals with the player's turn, executes when a card is placed into the dropzone
    public IEnumerator PlayerAttack(Card cardPlayed, int amnt)
    {
        Debug.Log("player relationship:" + playerUnit.relationship.getStatus());
        // playerUnit.shield = 0;
        bool isDead = false; // true if the opponent is dead, false otherwise
        string cardName = cardPlayed.name;
        // CardType type = cardPlayed.type;
        // Debug.Log(type);

        // Pay mana cost
        playerUnit.mana -= cardPlayed.cost;
        playerHUD.SetMana(playerUnit.mana);

        if (cardName == "Strike") // strike card is played
        {
            isDead = opponentUnit.TakeDamage(30); // deals damage to the opponent and returns true if the opponent is dead
            opponentHUD.SetHP(opponentUnit.HP); // update the opponent's HP
            opponentHUD.SetShield(opponentUnit.shield);
            dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 30 + " damage]"; // change the dialogue text
        }

        else if (cardName == "Guard") // guard card is played
        {
            playerUnit.Guard(15); // heals the player's shield
            playerHUD.SetShield(playerUnit.shield); // updates the player's shield
            dialogueText.text = "You gained [+" + 15 + " shield]"; // change the dialogue text
        }

        else if (cardName == "Recover") // recover card is played
        {
            playerUnit.Recover(15); // heals the player's hp
            playerHUD.SetHP(playerUnit.HP); // updates the player's hp
            dialogueText.text = "You gained [+" + 15 + " life]"; // change the dialogue text
        }

        else if (cardName == "Cross") // cross card is played
        {
            cross = true;
            dialogueText.text = "You will only take half of the next attack's damage"; // change the dialogue text
        }
        else if (cardName == "Burning Desire") // burn card is played
        {
            burnCount = 0;
            burn = true;
            dialogueText.text = "The opponent will take 60 damage total over the next three turns"; // change the dialogue text
        }
        else if (cardName == "Living On The Edge") // burn card is played
        {
            if (playerUnit.HP < 30)
            {
                isDead = opponentUnit.TakeDamage(60);
                dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 60 + " damage]"; // change the dialogue text
            }
            else
            {
                isDead = opponentUnit.TakeDamage(20);
                dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 20 + " damage]"; // change the dialogue text
            }
            opponentHUD.SetHP(opponentUnit.HP); // update the opponent's HP
            opponentHUD.SetShield(opponentUnit.shield);
        }
        else if (cardName == "Talk")
        {
            talksPlayed++;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + (talksPlayed - 1) * 2, LoadSceneMode.Additive);
        }

        yield return new WaitForSeconds(2f); // waits for two seconds

        if (isDead) // if the opponent is dead
        {
            state = BattleState.WON; // change the battle state
            EndBattle(); // run endbattle function
        }
        else if (SceneManager.GetActiveScene() != gameObject.scene) // Pause if talk card was played, resume if talk scene is exited
        {
            yield return new WaitWhile(() => SceneManager.GetActiveScene() != gameObject.scene);
            dialogueText.text = "";
            playerUnit.relationship.setStatus(opponentUnit.charName);
            /*             Debug.Log(TalkChoice.getChoice());
                        playerUnit.relationship.setStatus(TalkChoice.getChoice());
                        TalkChoice.setChoice(0); */
            //StartCoroutine(OpponentAttack());
            EndTurn();
        }
        else
        {
            // StartCoroutine(OpponentAttack()); // start opponent attack coroutine
        }
    }

    // Possibly move to seperate script?
    public void EndTurn()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(OpponentAttack());
        }
    }


    // IEnumerator PlayerTalk()
    // {
    //     //dialogueText.text = "we do be talking"; // changes the dialogue text
    //     yield return new WaitForSeconds(2f); // waits two seconds

    //     StartCoroutine(OpponentAttack()); // starts the opponent attack coroutine
    // }

    public IEnumerator OpponentAttack()
    {
        // TODO: add opponent mana cost and logic
        state = BattleState.OPPONENTTURN; // change the battle state
        bool opponentDead = false;

        // close the skill card menu if open
        // TODO: enable this w/ merge
        // if (menuButton.menu.gameObject.activeInHierarchy)
        // {
        //     menuButton.menu.gameObject.SetActive(false);
        // }

        GenerateMana(opponentUnit);
        opponentHUD.SetMana(opponentUnit.mana);
        turnText.text = opponentUnit.charName + "'s Turn";
        dialogueText.text = "";
        if (burnCount < 3 && burn)
        {
            yield return new WaitForSeconds(2f); // waits for two seconds
            dialogueText.text = opponentUnit.charName + " took damage from their Burn [- 20 health]"; // changes the dialogue text
            opponentDead = opponentUnit.TakeDamage(20);
            opponentHUD.SetHP(opponentUnit.HP);
            opponentHUD.SetShield(opponentUnit.shield);
            burnCount++;
            if (opponentDead) // if the opponent is dead
            {
                state = BattleState.WON; // change the battle state
                EndBattle(); // run endbattle function
            }
        }

        yield return new WaitForSeconds(2f); // waits for two seconds
        opponentUnit.Play(); // calls function where opponent chooses a random card 
        string card = opponentUnit.cardToPlay;
        Debug.Log(opponentUnit.cardToPlay + " played by opponent");

        bool isDead = false;

        if (card == "Strike") // strike card is played
        {
            if (cross)
            {
                isDead = playerUnit.TakeDamage(15);
                cross = false;
                dialogueText.text = opponentUnit.charName + "'s attack hit you for [-" + 15 + " damage]"; // change the dialogue text
            }
            else
            {
                isDead = playerUnit.TakeDamage(30); // deals damage to the player and returns true if the player is dead
                dialogueText.text = opponentUnit.charName + "'s attack hit you for [-" + 30 + " damage]"; // change the dialogue text
            }
            playerHUD.SetHP(playerUnit.HP); // update the player's HP
            playerHUD.SetShield(playerUnit.shield); // update the player's shield
        }

        else if (card == "Guard") // guard card is played
        {
            opponentUnit.Guard(15); // heals the opponent's shield
            opponentHUD.SetShield(opponentUnit.shield); // updates the opponent's shield
            dialogueText.text = opponentUnit.charName + " gained [+" + 15 + " shield]"; // change the dialogue text
        }

        else if (card == "Recover") // recover card is played
        {
            Debug.Log("entered");
            opponentUnit.Recover(15); // heals the opponent's hp
            opponentHUD.SetHP(opponentUnit.HP); // updates the opponents's hp
            dialogueText.text = opponentUnit.charName + " gained [+" + 15 + " life]"; // change the dialogue text
            Debug.Log("healed");
        }
        Debug.Log(state + " hello " + isDead);
        yield return new WaitForSeconds(2f); // waits for two seconds
        Debug.Log(state + " and " + isDead);
        if (isDead) // if the player is dead
        {
            state = BattleState.LOST; // change the battle state
            EndBattle(); // run endbattle function
        }
        else
        {
            PlayerTurn(); // start playerturn function
        }
    }

    void GenerateMana(Duelist duelist)
    {
        if (turnNum > 6)
        {
            duelist.mana += 4;
        }
        else if (turnNum > 3)
        {
            duelist.mana += 3;
        }
        else
        {
            duelist.mana += 2;
        }
    }

    public void ManaForCard()
    {
        if (state == BattleState.PLAYERTURN)
        {
            playerUnit.mana -= 2;
            playerHUD.SetMana(playerUnit.mana);
            playerUnit.deck.Draw();
        }
    }

    void PlayerTurn()
    {
        // TODO: enable this
        // menuButton.updateCards();
        turnNum++;
        GenerateMana(playerUnit);
        playerHUD.SetMana(playerUnit.mana);
        state = BattleState.PLAYERTURN;
        opponentUnit.EndTurn();
        turnText.text = "Your Turn";
        dialogueText.text = "";
        playerUnit.deck.Draw();
    }

    public IEnumerator SkipTurn()
    {
        dialogueText.text = "You skipped your turn.";
        yield return new WaitForSeconds(2f); // waits two seconds
        StartCoroutine(OpponentAttack());
    }

    public void OnSkipTurnButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(SkipTurn());
    }

    void EndBattle()
    {
        gameOverUI.gameOverScreen.SetActive(true);
        
        if (state == BattleState.WON) // if the player has won
        {
            gameOverUI.LoadCard(playerUnit.relationship, opponentUnit.charName, playerUnit.deck);
            wonLost.text = "You defeated " + opponentUnit.charName + "!"; // change the dialogue text
        }
        else if (state == BattleState.LOST) // if the player has lost
        {
            wonLost.text = "You lost against " + opponentUnit.charName + "!"; // change the dialogue text
            SceneManager.LoadScene("GameOver");
        }
    }
}
