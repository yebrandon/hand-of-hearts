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

        yield return new WaitForSeconds(2f); // wait for two seconds

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

        yield return new WaitForSeconds(2f);

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
        dialogueText.text = "we do be talking";
        yield return new WaitForSeconds(2f);

        StartCoroutine(OpponentAttack());
    }

    IEnumerator OpponentAttack()
    {
        state = BattleState.OPPONENTTURN; // change the battle state
        dialogueText.text = opponentUnit.charName + " hits ya";

        yield return new WaitForSeconds(2f);

        opponentUnit.Play();
        bool isDead = playerUnit.TakeDamage(opponentUnit.damage);

        playerHUD.SetHP(playerUnit.HP);
        playerHUD.SetShield(playerUnit.shield);

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
