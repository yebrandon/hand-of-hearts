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
    public GameObject drawCardButton;
    public GameObject handCardArea;

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
    public bool sticky = false;
    public bool sugar = false;
    
    public int burnCount = 0;
    public int sugarCount = 0;

    void Start()
    {
        SceneTracker.lastBattleSceneName = SceneManager.GetActiveScene().name;
        SceneTracker.lastBattleCharName = opponentUnit.charName;
        turnNum = 0;
        talksPlayed = 0;

        state = BattleState.START;
        gameOverUI.gameOverScreen.SetActive(false);
        SetUpBattle();
        StartCoroutine(SetUpBattle());
    }


    // Set up initial turn and log text, player and opponent UI, and relationship status
    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Player>();

        // Set relationship status to STRANGERS
        Relationships.relationships[opponentUnit.charName] = RelationshipStatus.STRANGERS;
        Debug.Log(Relationships.relationships[opponentUnit.charName]);
        playerUnit.relationship.setStatus(opponentUnit.charName);

        turnText.text = "Your Turn";
        dialogueText.text = "The battle begins!";

        playerHUD.SetHUD(playerUnit);
        opponentHUD.SetHUD(opponentUnit);
        disableButtons(); // disable end turn and draw card buttons
        playerUnit.relationship.setStatus(opponentUnit.charName);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }


    // Executes when a card is placed into the dropzone
    public IEnumerator PlayerAttack(Card cardPlayed)
    {
        if (sticky)
        {
            sticky = false;
            EndTurn();
        }
        
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
        else if (cardName == "Blue Morpho Butterfly")
        {
            playerUnit.mana += cardPlayed.effect;
            if (playerUnit.mana > 10)
            {
                playerUnit.mana = 10;
            }
            playerHUD.SetMana(playerUnit.mana);
            dialogueText.text = "You gained " + (cardPlayed.effect - 2).ToString() + " mana!";
        }
        else if (cardName == "Candied")
        {
            dialogueText.text = "Your card did nothing.";
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

            playerUnit.relationship.setStatus(opponentUnit.charName);
            EndTurn();
        }
    }

    // Possibly move to seperate script?
    public void EndTurn()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(OpponentTurn());
        }
    }

    // Opponent chooses what to do on their turn
    public IEnumerator OpponentPlay()
    {
        CardDisplay [] playerCards = handCardArea.GetComponentsInChildren<CardDisplay>();
        // Opponent chooses an action to take
        string action = opponentUnit.ChooseAction();
        if (action == "EndTurn")
        {
            dialogueText.text = opponentUnit.charName + " ended their turn.";
            yield return new WaitForSeconds(2f);
            PlayerTurn();
            yield break;
        }
        else if (action == "Draw")
        {
            opponentUnit.mana -= 3;
            opponentHUD.SetMana(opponentUnit.mana);
            opponentUnit.Draw();
            dialogueText.text = opponentUnit.charName + " spent 3 mana to draw a card!";
            yield return new WaitForSeconds(2f);
            StartCoroutine(OpponentPlay());
        }
        else if (action == "Card")
        {
            bool isDead = false;
            Card cardToPlay = opponentUnit.cardDisplay.card;

            // Pay mana cost
            opponentUnit.mana -= cardToPlay.cost;
            opponentHUD.SetMana(opponentUnit.mana);

            dialogueText.text = opponentUnit.charName + " played " + cardToPlay.name + "!";
            yield return new WaitForSeconds(2f);

            // Card effects
            if (cardToPlay.name == "Blue Morpho Butterfly")
            {
                opponentUnit.mana += 5;
                if (opponentUnit.mana > 10)
                {
                    opponentUnit.mana = 10;
                }
                opponentHUD.SetMana(opponentUnit.mana);
                dialogueText.text = "Constants gained 5 mana!";
            }
            else if (cardToPlay.name == "Hofstadter's Butterfly")
            {
                if (opponentUnit.MAX_HAND_SIZE - opponentUnit.hand.Count == 1)
                {
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    dialogueText.text = "Constants added one copy of " + opponentUnit.lastPlayedCardName + " to his hand!";
                }
                else
                {
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    dialogueText.text = "Constants added two copies of " + opponentUnit.lastPlayedCardName + " to his hand!";
                }
                opponentUnit.clearCardZone();
                yield return new WaitForSeconds(2f);
                PlayerTurn();
                yield break;
            }
            else if (cardToPlay.name == "Chaos")
            {
                int dmg = 10 * opponentUnit.numButterfliesPlayed;
                isDead = dealDamage("Chaos", dmg);

                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                opponentUnit.hand.RemoveAll(cardName => cardName.Contains("Chaos"));
            }
            else if (cardToPlay.name == "Candied")
            {
                int cardIndex = Random.Range(0, playerCards.Length - 1);
                Vector3 pos = playerCards[cardIndex].transform.localPosition;
                Destroy(playerCards[cardIndex].gameObject);

                GameObject candiedCard = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerCards/CandiedPlayer"));
                candiedCard.transform.SetParent(handCardArea.transform);
                candiedCard.transform.localPosition = pos;
                candiedCard.transform.localScale = new Vector3(1f, 1f, 1f);
                dialogueText.text = "You've been Candied! Your " + playerCards[cardIndex].card.name + " card has been replaced with a Candied card!";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Toothache")
            {
                RelationshipStatus playerRelationship = playerUnit.relationship.getStatus();
                int dmg = 0;
                if (playerRelationship == RelationshipStatus.STRANGERS)
                {
                    dmg = 60;
                }
                else if (playerRelationship == RelationshipStatus.ACQUAINTANCES)
                {
                    dmg = 50;
                }
                else if (playerRelationship == RelationshipStatus.FRIENDS)
                {
                    dmg = 40;
                }
                else if (playerRelationship == RelationshipStatus.FRIENDS)
                {
                    dmg = 30;
                }
                isDead = dealDamage("Toothache", dmg);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
            }
            else if (cardToPlay.name == "Sticky Situation")
            {
                sticky = true;
                dialogueText.text = "Candy's card skips your next turn.";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Sugar Rush")
            {
                sugar = true;
                dialogueText.text = "Candy heals 20 life over the next two turns.";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "pog")
            {
                int dmg = playerCards.Length*5;
                dialogueText.text = "You took 5 damage for each card in your hand!";
                yield return new WaitForSeconds(2f);
                isDead = dealDamage("pog", dmg);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
            }
            else if (cardToPlay.name == "Exchangemint")
            {
                dialogueText.text = "Candy switches health values with you!";
                yield return new WaitForSeconds(0.5f);
                int playerHP = playerUnit.HP;
                playerUnit.HP = opponentUnit.HP;
                opponentUnit.HP = playerHP;
                playerHUD.SetHP(playerUnit.HP);
                opponentHUD.SetHP(opponentUnit.HP);
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Veil of Thorns")
            {
                dialogueText.text = "Rosa will reflect damage back to you for two of your turns!";
                Debug.Log("Veil of Thorns");
            }
            else if (cardToPlay.name == "Lament")
            {
                dialogueText.text = "Rosa lost 10 HP and gained 20 shield!";
                opponentUnit.HP -= 10;
                opponentHUD.SetHP(opponentUnit.HP);
                opponentUnit.Guard(20);
                opponentHUD.SetShield(opponentUnit.shield);
            }
            else if (cardToPlay.name == "Garden of None")
            {
                dialogueText.text = "Rosa expended " + opponentUnit.shield + " shield to deal " + opponentUnit.shield + " damage!";
                isDead = playerUnit.TakeDamage(opponentUnit.shield);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                opponentUnit.shield = 0;
                opponentHUD.SetShield(opponentUnit.shield);
            }
            else if (cardToPlay.name == "Fate's Wreath")
            {
                dialogueText.text = "Rosa gained 100 shield and added a copy of Fate\'s Wreath to their hand!";
                opponentUnit.Guard(100);
                opponentHUD.SetShield(opponentUnit.shield);
                opponentUnit.hand.Add("Fate's Wreath");
            }

            opponentUnit.lastPlayedCardName = cardToPlay.name;
            cross = false;
            if (isDead)
            {
                state = BattleState.LOST;
                dialogueText.text = "Your health has been depleted to 0!";
                yield return new WaitForSeconds(2f);
                EndBattle();
            }
            else
            {
                opponentUnit.clearCardZone();
                yield return new WaitForSeconds(2f);
                StartCoroutine(OpponentPlay());
            }
        }
    }

    // Opponent's Turn
    public IEnumerator OpponentTurn()
    {
        state = BattleState.OPPONENTTURN;
        bool opponentDead = false;
        // Close the skill card menu if open
        if (menuButton.menu.gameObject.activeInHierarchy)
        {
            menuButton.menu.gameObject.SetActive(false);
        }

        GenerateMana(opponentUnit);
        opponentHUD.SetMana(opponentUnit.mana);

        turnText.text = opponentUnit.charName + "'s Turn";
        dialogueText.text = "";

        opponentUnit.Draw();

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
                yield return new WaitForSeconds(2f); // waits for two seconds
                state = BattleState.WON; // change the battle state
                EndBattle(); // run endbattle function
                yield break;
            }
        }
        if (sugarCount < 2 && sugar)
        {
            yield return new WaitForSeconds(2f); // waits for two seconds
            dialogueText.text = opponentUnit.charName + " healed from her Sugar Rush [+ 10 health]"; // changes the dialogue text
            opponentUnit.Recover(10);
            opponentHUD.SetHP(opponentUnit.HP);
            opponentHUD.SetShield(opponentUnit.shield);
            sugarCount++;
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(OpponentPlay());
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
        if (duelist.mana > 10)
        {
            duelist.mana = 10;
        }
    }


    public void ManaForCard()
    {
        if (state == BattleState.PLAYERTURN && playerUnit.deck.hand.transform.childCount < playerUnit.deck.MAX_HAND_SIZE && playerUnit.mana >= 3)
        {
            playerUnit.mana -= 3;
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
        enableButtons();
    }

    public bool dealDamage(string attackName, int dmg)
    {
        if (cross)
            {
                bool isDead = playerUnit.TakeDamage(dmg / 2);
                dialogueText.text = "Because of your Cross card," + opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)(dmg / 2) + " damage]";
                cross = false;
                return isDead;
            }
            else
            {
                bool isDead = playerUnit.TakeDamage(dmg / 2);
                dialogueText.text = opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)(dmg) + " damage]";
                return isDead;
            }
    }

    public void enableButtons()
    {
        endTurnButton.GetComponent<Button>().interactable = true;
        drawCardToggle();
        menuButton.GetComponent<Button>().interactable = true;
    }

    public void disableButtons()
    {
        endTurnButton.GetComponent<Button>().interactable = false;
        drawCardToggle();
        menuButton.GetComponent<Button>().interactable = false;
    }

    public void drawCardToggle()
    {
        if (playerUnit.mana >= 3)
        {
            drawCardButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            drawCardButton.GetComponent<Button>().interactable = false;
        }
    }


    void EndBattle()
    {
        if (state == BattleState.WON) // if the player has won
        {

            gameOverUI.gameOverScreen.SetActive(true);
            disableButtons();
            gameOverUI.LoadCard(playerUnit.relationship, opponentUnit.charName, playerUnit.deck);
            wonLost.text = "You defeated " + opponentUnit.charName + "!"; // change the dialogue text
        }
        else if (state == BattleState.LOST) // if the player has lost
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
