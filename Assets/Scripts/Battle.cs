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
        state = BattleState.START;
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }

    // Update is called once per frame
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Player>();

        dialogueText.text = "Woah, " + opponentUnit.charName + " appeared!";

        playerHUD.SetHUD(playerUnit);
        opponentHUD.SetHUD(opponentUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public IEnumerator PlayerAttack(int dmg)
    {
        Debug.Log("playerattack");
        Debug.Log(opponentUnit.HP);
        bool isDead = opponentUnit.TakeDamage(dmg);

        opponentHUD.SetHP(opponentUnit.HP);
        dialogueText.text = "the attack hit em";
        Debug.Log(opponentUnit.HP);
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.OPPONENTTURN;
            StartCoroutine(OpponentAttack());
        }
    }

    public IEnumerator PlayerDefend(int amnt)
    {
        playerUnit.shield += amnt;
        playerHUD.SetShield(playerUnit.shield);

        yield return new WaitForSeconds(2f);

        state = BattleState.OPPONENTTURN;
        StartCoroutine(OpponentAttack());
    }

    public IEnumerator PlayerHeal(int amnt)
    {
        Debug.Log('a');
        if (playerUnit.HP + amnt >= playerUnit.maxHP)
        {
            playerUnit.HP = playerUnit.maxHP;
            playerHUD.SetHP(playerUnit.HP);
        }
        else
        {
            playerUnit.HP += amnt;
            playerHUD.SetHP(playerUnit.HP);
        }

        yield return new WaitForSeconds(2f);

        state = BattleState.OPPONENTTURN;
        StartCoroutine(OpponentAttack());
    }


    IEnumerator PlayerTalk()
    {
        dialogueText.text = "we do be talking";
        yield return new WaitForSeconds(2f);

        StartCoroutine(OpponentAttack());
    }

    IEnumerator OpponentAttack()
    {
        dialogueText.text = opponentUnit.charName + " hits ya";

        yield return new WaitForSeconds(2f);

        opponentUnit.Play();

        yield return new WaitForSeconds(2f);

        PlayerTurn();
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
        if (state == BattleState.WON)
        {
            dialogueText.text = "congrats u won against " + opponentUnit.charName;
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "u lost against " + opponentUnit.charName + ", wow whatta loser";
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack(10));
    }

    public void OnTalkButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerTalk());
    }
}
