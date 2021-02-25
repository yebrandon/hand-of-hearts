using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START; // set to the start state when the first frame is updated
        // set up the battle and start the coroutine
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }

    // sets up the battle by setting up the player and opponent GUI
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab); // instantiating the player's gameobject
        //playerGO.transform.SetParent(playerBattleStation);
        playerUnit = playerGO.GetComponent<Player>(); // gets the player's component

        dialogueText.text = "Woah, " + opponentUnit.charName + " appeared!"; // sets the dialogue text

        playerHUD.SetHUD(playerUnit); // setup player HUD
        opponentHUD.SetHUD(opponentUnit); // setup opponent HUD

        yield return new WaitForSeconds(2f); // waits for two seconds

        state = BattleState.PLAYERTURN; // change the battle state to be the player's turn
        PlayerTurn(); // run the player's turn function
    }

    // deals with the player's turn, executes when a card is placed into the dropzone
    public IEnumerator PlayerAttack(string card, int amnt)
    {
        bool isDead = false; // true if the opponent is dead, false otherwise

        if (card == "Strike") // strike card is played
        {
            isDead = opponentUnit.TakeDamage(50); // deals damage to the opponent and returns true if the opponent is dead
            opponentHUD.SetHP(opponentUnit.HP); // update the opponent's HP
            opponentHUD.SetShield(opponentUnit.shield);
            dialogueText.text = "The attack hit " + opponentUnit.charName + " [-" + 50 + " hp]"; // change the dialogue text
        }

        else if (card == "Guard") // guard card is played
        {
            playerUnit.Guard(10); // heals the player's shield
            playerHUD.SetShield(playerUnit.shield); // updates the player's shield
            dialogueText.text = "Your shield has been healed [+" + 10 + " shield]"; // change the dialogue text
        }

        else if (card == "Recover") // recover card is played
        {
            playerUnit.Recover(30); // heals the player's hp
            playerHUD.SetHP(playerUnit.HP); // updates the player's hp
            dialogueText.text = "Your HP has been healed [+" + 30 + " hp]"; // change the dialogue text
        }
        else if (card == "Talk")
        {
            dialogueText.text = "WE DO BE TALKIN THO";
        }

        yield return new WaitForSeconds(2f); // waits for two seconds

        if (isDead) // if the opponent is dead
        {
            state = BattleState.WON; // change the battle state
            EndBattle(); // run endbattle function
        }
        else
        {
            StartCoroutine(OpponentAttack()); // start opponent attack coroutine
        }
    }

    IEnumerator PlayerTalk()
    {
        dialogueText.text = "we do be talking"; // changes the dialogue text
        yield return new WaitForSeconds(2f); // waits two seconds

        StartCoroutine(OpponentAttack()); // starts the opponent attack coroutine
    }

    IEnumerator OpponentAttack()
    {
        state = BattleState.OPPONENTTURN; // change the battle state
        dialogueText.text = opponentUnit.charName + "'s turn: "; // changes the dialogue text

        yield return new WaitForSeconds(2f); // waits for two seconds
        opponentUnit.Play(); // calls function where opponent chooses a random card 
        string card = opponentUnit.playCard;
        Debug.Log(opponentUnit.playCard + " played by opponent");

        bool isDead = false; // true if the player is dead, false otherwise

        if (card == "Strike") // strike card is played
        {
            isDead = playerUnit.TakeDamage(50); // deals damage to the player and returns true if the player is dead
            playerHUD.SetHP(playerUnit.HP); // update the player's HP
            playerHUD.SetShield(playerUnit.shield); // update the player's shield
            dialogueText.text = "The attack hit you for [-" + 50 + " hp]"; // change the dialogue text
        }

        else if (card == "Guard") // guard card is played
        {
            opponentUnit.Guard(10); // heals the opponent's shield
            opponentHUD.SetShield(opponentUnit.shield); // updates the opponent's shield
            dialogueText.text = opponentUnit.charName + "'s shield has been healed [+" + 10 + " shield]"; // change the dialogue text
        }

        else if (card == "Recover") // recover card is played
        {
            opponentUnit.Recover(30); // heals the opponent's hp
            opponentHUD.SetHP(opponentUnit.HP); // updates the opponents's hp
            dialogueText.text = opponentUnit.charName + "'s HP has been healed [+" + 30 + " hp]"; // change the dialogue text
        }

        yield return new WaitForSeconds(2f); // waits for two seconds

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

    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        opponentUnit.EndTurn();
        dialogueText.text = "ur turn";
        playerUnit.deck.Draw();
    }

    void EndBattle()
    {
        if (state == BattleState.WON) // if the player has won
        {
            dialogueText.text = "congrats u won against " + opponentUnit.charName; // change the dialogue text
        }
        else if (state == BattleState.LOST) // if the player has lost
        {
            dialogueText.text = "u lost against " + opponentUnit.charName + ", wow whatta loser"; // change the dialogue text
        }
    }
}
