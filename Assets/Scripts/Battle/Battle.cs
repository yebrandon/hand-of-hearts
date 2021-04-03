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

    public Transform playerBattleStation;
    public Transform opponentBattleStation;

    public Player playerUnit;
    public Opponent opponentUnit;

    public BattleHUD playerHUD;
    public BattleHUD opponentHUD;

    public Text dialogueText;
    public Text turnText;

    public MenuButton menuButton;

    public int turnNum;
    public static int talksPlayed;

    public bool cross = false;
    public bool burn = false;
    public int burnCount = 0;

    void Start()
    {
        turnNum = 0;
        talksPlayed = 0;

        state = BattleState.START;
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }


    // Set up initial turn and log text, player and opponent UI, and relationship status
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Player>();

        // Change relationship status from NOTMET to strangers 
        Relationships.relationships[opponentUnit.charName]++;
        playerUnit.relationship.setStatus(opponentUnit.charName);

        turnText.text = "Your Turn";
        dialogueText.text = "The battle begins!";

        playerHUD.SetHUD(playerUnit);
        opponentHUD.SetHUD(opponentUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }


    // Executes when a card is placed into the dropzone
    public IEnumerator PlayerAttack(Card cardPlayed)
    {
        bool isDead = false;
        string cardName = cardPlayed.name;

        // Pay mana cost
        playerUnit.mana -= cardPlayed.cost;
        playerHUD.SetMana(playerUnit.mana);

        if (cardName == "Strike")
        {
            isDead = opponentUnit.TakeDamage(30); // deals damage to the opponent and returns true if the opponent is dead
            opponentHUD.SetHP(opponentUnit.HP);
            opponentHUD.SetShield(opponentUnit.shield);
            dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 30 + " damage]";
        }

        else if (cardName == "Guard")
        {
            playerUnit.Guard(15);
            playerHUD.SetShield(playerUnit.shield);
            dialogueText.text = "You gained [+" + 15 + " shield]";
        }

        else if (cardName == "Recover")
        {
            playerUnit.Recover(15);
            playerHUD.SetHP(playerUnit.HP);
            dialogueText.text = "You gained [+" + 15 + " life]";
        }

        else if (cardName == "Cross")
        {
            cross = true;
            dialogueText.text = "You will only take half of the next attack's damage";
        }
        else if (cardName == "Burning Desire")
        {
            burnCount = 0;
            burn = true;
            dialogueText.text = "The opponent will take 60 damage total over the next three turns";
        }
        else if (cardName == "Living On The Edge")
        {
            if (playerUnit.HP < 30)
            {
                isDead = opponentUnit.TakeDamage(60);
                dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 60 + " damage]";
            }
            else
            {
                isDead = opponentUnit.TakeDamage(20);
                dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 20 + " damage]";
            }
            opponentHUD.SetHP(opponentUnit.HP);
            opponentHUD.SetShield(opponentUnit.shield);
        }
        else if (cardName == "Talk")
        {
            talksPlayed++;

            // Load the appropriate talk card scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + (talksPlayed - 1) * 2, LoadSceneMode.Additive);
        }

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else if (SceneManager.GetActiveScene() != gameObject.scene) // Pause if talk card was played, resume if talk scene is exited
        {
            yield return new WaitWhile(() => SceneManager.GetActiveScene() != gameObject.scene);

            dialogueText.text = "";
            playerUnit.relationship.setStatus(opponentUnit.charName);
            EndTurn();
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


    // Opponent's Turn
    public IEnumerator OpponentAttack()
    {
        // TODO: add opponent mana cost and logic

        state = BattleState.OPPONENTTURN;

        // Close the skill card menu if open
        if (menuButton.menu.gameObject.activeInHierarchy)
        {
            menuButton.menu.gameObject.SetActive(false);
        }

        GenerateMana(opponentUnit);
        opponentHUD.SetMana(opponentUnit.mana);

        turnText.text = opponentUnit.charName + "'s Turn";
        dialogueText.text = "";

        if (burnCount < 3 && burn)
        {
            yield return new WaitForSeconds(2f);
            dialogueText.text = opponentUnit.charName + " took damage from their Burn [- 20 health]";
            opponentUnit.TakeDamage(20);
            opponentHUD.SetHP(opponentUnit.HP);
            opponentHUD.SetShield(opponentUnit.shield);
            burnCount++;
        }

        yield return new WaitForSeconds(2f);

        // Opponent chooses a random card to play
        opponentUnit.Play();
        string card = opponentUnit.cardToPlay;

        bool isDead = false;

        if (card == "Strike")
        {
            // TODO: Move cross logic to work on all offensive cards rather than only Strike?
            if (cross)
            {
                isDead = playerUnit.TakeDamage(15);
                cross = false;
                dialogueText.text = opponentUnit.charName + "'s attack hit you for [-" + 15 + " damage]";
            }
            else
            {
                isDead = playerUnit.TakeDamage(30); // deals damage to the player and returns true if the player is dead
                dialogueText.text = opponentUnit.charName + "'s attack hit you for [-" + 30 + " damage]";
            }
            playerHUD.SetHP(playerUnit.HP);
            playerHUD.SetShield(playerUnit.shield);
        }

        else if (card == "Guard")
        {
            opponentUnit.Guard(15);
            opponentHUD.SetShield(opponentUnit.shield);
            dialogueText.text = opponentUnit.charName + " gained [+" + 15 + " shield]";
        }

        else if (card == "Recover")
        {
            opponentUnit.Recover(15);
            opponentHUD.SetHP(opponentUnit.HP);
            dialogueText.text = opponentUnit.charName + " gained [+" + 15 + " life]";
        }

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            PlayerTurn();
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
        menuButton.updateCards();
        turnNum++;

        GenerateMana(playerUnit);
        playerHUD.SetMana(playerUnit.mana);

        state = BattleState.PLAYERTURN;
        opponentUnit.EndTurn();

        turnText.text = "Your Turn";
        dialogueText.text = "";

        playerUnit.deck.Draw();
    }


    // TODO: Remove if skip turn button is removed?
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
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
        }
        else if (state == BattleState.LOST)
        {
            SceneTracker.lastBattleSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameOver");
        }
    }
}
