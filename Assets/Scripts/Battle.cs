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

    Duelist playerUnit;
    Duelist opponentUnit;

    public BattleHUD playerHUD;
    public BattleHUD opponentHUD;

    public Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }

    // Update is called once per frame
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Duelist>();
        
        GameObject opponentGO = Instantiate(opponentPrefab, opponentBattleStation);
        opponentUnit = opponentGO.GetComponent<Duelist>();

        dialogueText.text = "Woah, " + opponentUnit.charName + " appeared!";

        playerHUD.SetHUD(playerUnit);
        opponentHUD.SetHUD(opponentUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack() {
        bool isDead = opponentUnit.TakeDamage(playerUnit.damage);

        opponentHUD.SetHP(opponentUnit.HP);
        dialogueText.text = "the attack hit em";

        yield return new WaitForSeconds(2f);

        if (isDead){
            state = BattleState.WON;
            EndBattle();
        }
        else {
            state = BattleState.OPPONENTTURN;
            StartCoroutine(OpponentAttack());
        }
    }

    IEnumerator PlayerTalk () {
        dialogueText.text = "we do be talking";
        yield return new WaitForSeconds(2f);

        StartCoroutine(OpponentAttack());
    }

    IEnumerator OpponentAttack() {
            dialogueText.text = opponentUnit.charName + " hits ya";

            yield return new WaitForSeconds(2f);

            bool isDead = playerUnit.TakeDamage(opponentUnit.damage);

            playerHUD.SetHP(playerUnit.HP);

            yield return new WaitForSeconds(2f);

            if (isDead){
                state = BattleState.LOST;
                EndBattle();
            }
            else {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

    void PlayerTurn () {
        dialogueText.text = "ur turn";
    }

    void EndBattle() {
        if (state == BattleState.WON) {
            dialogueText.text = "congrats u won against " + opponentUnit.charName;
        }
        else if (state == BattleState.LOST) {
            dialogueText.text = "u lost against " + opponentUnit.charName + ", wow whatta loser";
        }
    }

    public void OnAttackButton(){
        if (state != BattleState.PLAYERTURN){
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnTalkButton(){
        if (state != BattleState.PLAYERTURN){
            return;
        }

        StartCoroutine(PlayerTalk());
    }
}
