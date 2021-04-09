using System.Collections;
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
    public bool veil = false;
    public bool boid = false;
    public bool cloak = false;
    public bool sneak = false;

    public int burnCount = 0;
    public int sugarCount = 0;
    public int veilCount = 0;
    public int veilDamage = 0;

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
        bool isDead = false;
        bool playerDead = false;
        string cardName = cardPlayed.name;

        // Pay mana cost
        playerUnit.mana -= cardPlayed.cost;
        drawCardToggle();
        playerHUD.SetMana(playerUnit.mana);

        if (cardName == "Strike")
        {
            if (veilCount < 2 && veil)
            {
                dialogueText.text = opponentUnit.charName + " reflected the attack back at you!";
                yield return new WaitForSeconds(2f);
                if (cross)
                {
                    dialogueText.text = "Because of your Cross card, you took less damage from your own Strike! [-10 life]";
                    playerDead = playerUnit.TakeDamage(10);
                    cross = false;
                }
                else
                {
                    dialogueText.text = "You took damage from your own Strike! [-20 life]";
                    playerDead = playerUnit.TakeDamage(20);
                    yield return new WaitForSeconds(2f);
                }
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                yield return new WaitForSeconds(2f);
                if (playerDead)
                {
                    yield return new WaitForSeconds(2f);
                    state = BattleState.LOST;
                    EndBattle();
                    yield break;
                }
            }
            else
            {
                isDead = dealDamageBoid("Strike", 20);
                opponentHUD.SetHP(opponentUnit.HP);
                opponentHUD.SetShield(opponentUnit.shield);
                // dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 30 + " life]";
                yield return new WaitForSeconds(2f);
            }
        }
        else if (cardName == "Guard")
        {
            playerUnit.Guard(15);
            playerHUD.SetShield(playerUnit.shield);
            dialogueText.text = "You gained [+" + 15 + " shield]";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Recover")
        {
            playerUnit.Recover(15);
            playerHUD.SetHP(playerUnit.HP);
            dialogueText.text = "You gained [+" + 15 + " life]";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Cross")
        {
            cross = true;
            dialogueText.text = "You will only take half of the next attack's damage";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Burning Desire")
        {
            burnCount = 0;
            burn = true;
            dialogueText.text = "The opponent will take 60 damage total over the next three turns";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Living On The Edge")
        {
            if (playerUnit.HP < 30)
            {
                if (veilCount < 2 && veil)
                {
                    dialogueText.text = opponentUnit.charName + " reflected the attack back at you!";
                    yield return new WaitForSeconds(2f);
                    if (cross)
                    {
                        dialogueText.text = "Because of your Cross card, you took less damage from your own Living on the Edge! [-30 life]";
                        yield return new WaitForSeconds(2f);
                        dialogueText.text = "";
                        playerDead = playerUnit.TakeDamage(30);
                        cross = false;
                    }
                    else
                    {
                        dialogueText.text = "You took damage from your own Living on the Edge! [-60 life]";
                        yield return new WaitForSeconds(2f);
                        dialogueText.text = "";
                        playerDead = playerUnit.TakeDamage(60);
                    }
                    playerHUD.SetHP(playerUnit.HP);
                    playerHUD.SetShield(playerUnit.shield);
                    if (playerDead)
                    {
                        yield return new WaitForSeconds(2f);
                        state = BattleState.LOST;
                        EndBattle();
                        yield break;
                    }
                }
                else
                {
                    isDead = dealDamageBoid("Living On The Edge", 60);
                    dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 60 + " life]";
                }
            }
            else
            {
                if (veilCount < 2 && veil)
                {
                    dialogueText.text = opponentUnit.charName + " reflected the attack back at you!";
                    yield return new WaitForSeconds(2f);
                    if (cross)
                    {
                        dialogueText.text = "Because of your Cross card, you took less damage from your own Living on the Edge! [-10 life]";
                        yield return new WaitForSeconds(2f);
                        dialogueText.text = "";
                        playerDead = playerUnit.TakeDamage(10);
                        cross = false;
                    }
                    else
                    {
                        dialogueText.text = "You took damage from your own Living on the Edge! [-20 life]";
                        yield return new WaitForSeconds(2f);
                        dialogueText.text = "";
                        playerDead = playerUnit.TakeDamage(20);
                    }
                    playerHUD.SetHP(playerUnit.HP);
                    playerHUD.SetShield(playerUnit.shield);
                    if (playerDead)
                    {
                        yield return new WaitForSeconds(2f);
                        state = BattleState.LOST;
                        EndBattle();
                        yield break;
                    }
                }
                else
                {
                    isDead = dealDamageBoid("Living On The Edge", 20);
                    dialogueText.text = "Your attack hit " + opponentUnit.charName + " for [-" + 20 + " life]";
                    yield return new WaitForSeconds(2f);
                }
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
            dialogueText.text = "You gained " + (cardPlayed.effect).ToString() + " mana!";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Candied")
        {
            dialogueText.text = "Your card did nothing.";
            yield return new WaitForSeconds(2f);
        }
        else if (cardName == "Exchangemint")
        {
            dialogueText.text = "You switched health values with " + opponentUnit.charName + "!";
            yield return new WaitForSeconds(0.5f);
            int playerHP = playerUnit.HP;
            playerUnit.HP = opponentUnit.HP;
            opponentUnit.HP = playerHP;
            playerHUD.SetHP(playerUnit.HP);
            opponentHUD.SetHP(opponentUnit.HP);
            yield return new WaitForSeconds(2f);

            if (cardPlayed.effect == 4)
            {
                dialogueText.text = "You also switched shield values with " + opponentUnit.charName + "!";
                yield return new WaitForSeconds(0.5f);
                int playerShield = playerUnit.shield;
                playerUnit.shield = opponentUnit.shield;
                opponentUnit.shield = playerShield;
                playerHUD.SetShield(playerUnit.shield);
                opponentHUD.SetShield(opponentUnit.shield);
                yield return new WaitForSeconds(2f);
            }
        }
        else if (cardName == "Boid")
        {
            playerUnit.playerBoid = cardPlayed.effect / 100.0;
            dialogueText.text = "You will block " + cardPlayed.effect + "% of the damage from your opponent's next attack!";
            yield return new WaitForSeconds(2f);

        }
        else if (cardName == "Talk")
        {
            talksPlayed++;

            // Load the appropriate talk card scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + (talksPlayed - 1) * 2, LoadSceneMode.Additive);
        }

        dialogueText.text = "";
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
            // Remove cloak effect
            if (cloak)
            {
                CardDisplay[] playerCards = handCardArea.GetComponentsInChildren<CardDisplay>();
                foreach (var x in playerCards)
                {
                    if (x.blockedByCloak)
                    {
                        x.blockedByCloak = false;
                    }
                };
                cloak = false;
            }

            if (veilCount < 2 && veil)
            {
                veilCount++;
            }
            else if (veilCount >= 2 && veil)
            {
                veilCount = 0;
                veil = false;
            }
            state = BattleState.OPPONENTTURN;
            disableButtons();
            StartCoroutine(OpponentTurn());
        }
    }

    // Opponent chooses what to do on their turn
    public IEnumerator OpponentPlay()
    {
        CardDisplay[] playerCards = handCardArea.GetComponentsInChildren<CardDisplay>();

        // Opponent chooses an action to take
        string action = opponentUnit.ChooseAction(playerUnit.HP);
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

            /*  ********
                ********
                CARD EFFECTS
                ********
                ********
            */
            // Constants
            if (cardToPlay.name == "Blue Morpho Butterfly")
            {
                opponentUnit.mana += 5;
                if (opponentUnit.mana > 10)
                {
                    opponentUnit.mana = 10;
                }
                opponentHUD.SetMana(opponentUnit.mana);
                dialogueText.text = "Constants gained 5 mana!";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Hofstadter's Butterfly")
            {
                if (opponentUnit.MAX_HAND_SIZE - opponentUnit.hand.Count == 1)
                {
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    dialogueText.text = "Constants added one copy of " + opponentUnit.lastPlayedCardName + " to his hand!";
                    yield return new WaitForSeconds(2f);
                }
                else
                {
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    opponentUnit.hand.Add(opponentUnit.lastPlayedCardName);
                    dialogueText.text = "Constants added two copies of " + opponentUnit.lastPlayedCardName + " to his hand!";
                    yield return new WaitForSeconds(2f);
                }
                opponentUnit.clearCardZone();
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
            // Candy 
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
                RelationshipStatus playerRelationship = Relationships.relationships[opponentUnit.charName];
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
                else if (playerRelationship == RelationshipStatus.HEARTWON)
                {
                    dmg = 30;
                }
                isDead = dealDamage("Toothache", dmg);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                yield return new WaitForSeconds(2f);
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
            else if (cardToPlay.name == "Sucker Punch")
            {
                int dmg = playerCards.Length * 10;
                dialogueText.text = "You took 10 damage for each card in your hand!";
                isDead = dealDamage("Sucker Punch", dmg);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Exchangemint")
            {
                dialogueText.text = "Candy switched health values with you!";
                yield return new WaitForSeconds(0.5f);
                int playerHP = playerUnit.HP;
                playerUnit.HP = opponentUnit.HP;
                opponentUnit.HP = playerHP;
                playerHUD.SetHP(playerUnit.HP);
                opponentHUD.SetHP(opponentUnit.HP);
                yield return new WaitForSeconds(2f);
            }
            // Jibb
            else if (cardToPlay.name == "Boid")
            {
                boid = true;
                dialogueText.text = "90% of the damage from your next attack against Jibb will be absorbed.";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Cloak")
            {
                cloak = true;
                dialogueText.text = "You are unable to attack Jibb for the next turn.";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Sneak Attack")
            {
                sneak = true;
                dialogueText.text = "Jibb's next attack will do double the damage.";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Quick Swipe")
            {
                isDead = dealDamage("Quick Swipe", 20);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Body Substitutes")
            {
                opponentUnit.Guard(15);
                opponentHUD.SetShield(opponentUnit.shield);
                dialogueText.text = "Jibb gained [+" + 15 + " shield]";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Stolen Potion")
            {
                opponentUnit.Recover(15);
                opponentHUD.SetHP(opponentUnit.HP);
                dialogueText.text = "Jibb gained [+" + 15 + " life]";
                yield return new WaitForSeconds(2f);
            }
            // Rosa
            else if (cardToPlay.name == "Veil of Thorns")
            {
                veil = true;
                dialogueText.text = "Rosa will reflect damage back to you for two of your turns!";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Lament")
            {
                opponentUnit.HP -= 10;
                opponentHUD.SetHP(opponentUnit.HP);
                opponentUnit.Guard(20);
                opponentHUD.SetShield(opponentUnit.shield);
                dialogueText.text = "Rosa lost 10 HP and gained 20 shield!";
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Garden of None")
            {
                isDead = dealDamage("Garden of None", opponentUnit.shield);
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                opponentUnit.shield = 0;
                opponentHUD.SetShield(opponentUnit.shield);
                yield return new WaitForSeconds(2f);
            }
            else if (cardToPlay.name == "Fate's Wreath")
            {
                opponentUnit.Guard(100);
                opponentHUD.SetShield(opponentUnit.shield);
                opponentUnit.hand.Add("Fate's Wreath");
                dialogueText.text = "Rosa gained 100 shield and added a copy of Garden of None to their hand!";
                yield return new WaitForSeconds(2f);
            }

            opponentUnit.lastPlayedCardName = cardToPlay.name;
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

        GenerateMana(opponentUnit);
        opponentHUD.SetMana(opponentUnit.mana);

        turnText.text = opponentUnit.charName + "'s Turn";
        dialogueText.text = "";

        opponentUnit.Draw();

        if (burnCount < 3 && burn)
        {
            if (veilCount < 2 && veil)
            {
                dialogueText.text = opponentUnit.charName + " reflected the attack back at you!";
                yield return new WaitForSeconds(2f);
                bool playerDead;
                if (cross)
                {
                    dialogueText.text = "Because of your Cross card, you took less damage from your own Burn! [-10 life]";
                    yield return new WaitForSeconds(2f);
                    dialogueText.text = "";
                    playerDead = playerUnit.TakeDamage(10);
                    cross = false;
                }
                else
                {
                    dialogueText.text = "You took damage from your own Burn! [-20 life]";
                    yield return new WaitForSeconds(2f);
                    dialogueText.text = "";
                    playerDead = playerUnit.TakeDamage(20);
                }
                playerHUD.SetHP(playerUnit.HP);
                playerHUD.SetShield(playerUnit.shield);
                if (playerDead)
                {
                    yield return new WaitForSeconds(2f);
                    state = BattleState.LOST;
                    EndBattle();
                    yield break;
                }
            }
            else if (boid)
            {
                dealDamageBoid("Burn", 20);
            }
            else
            {
                opponentDead = opponentUnit.TakeDamage(20);
                opponentHUD.SetHP(opponentUnit.HP);
                opponentHUD.SetShield(opponentUnit.shield);
                burnCount++;
                dialogueText.text = opponentUnit.charName + " took damage from their Burn [-20 life]";
                yield return new WaitForSeconds(2f);
                if (opponentDead)
                {
                    yield return new WaitForSeconds(2f);
                    state = BattleState.WON;
                    EndBattle();
                    yield break;
                }
            }

        }
        if (sugarCount < 2 && sugar)
        {
            yield return new WaitForSeconds(2f);
            dialogueText.text = opponentUnit.charName + " healed from her Sugar Rush [+ 10 health]";
            yield return new WaitForSeconds(2f);
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

        opponentUnit.EndTurn();

        state = BattleState.PLAYERTURN;

        if (sticky)
        {
            sticky = false;
            EndTurn();
            return;
        }

        turnText.text = "Your Turn";
        dialogueText.text = "";

        GenerateMana(playerUnit);
        playerHUD.SetMana(playerUnit.mana);

        playerUnit.deck.Draw();
        enableButtons();

        if (cloak)
        {
            CardDisplay[] playerCards = handCardArea.GetComponentsInChildren<CardDisplay>();
            foreach (var x in playerCards)
            {
                if (x.card.type == CardType.OFFENSIVE)
                {
                    x.blockedByCloak = true;
                }
            };
        }
    }

    public bool dealDamage(string attackName, int dmg)
    {
        if (cross && playerUnit.playerBoid > 0)
        {
            bool isDead = playerUnit.TakeDamage((int)((dmg / 2) * (1.0 - playerUnit.playerBoid)));
            dialogueText.text = "Because of your Cross and Boid cards, " + opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)((dmg / 2) * (1.0 - playerUnit.playerBoid)) + " life]";
            playerHUD.SetShield(playerUnit.shield);
            playerHUD.SetHP(playerUnit.HP);
            cross = false;
            playerUnit.playerBoid = 0;
            return isDead;
        }
        else if (cross)
        {
            bool isDead = playerUnit.TakeDamage((int)(dmg / 2));
            dialogueText.text = "Because of your Cross card, " + opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)(dmg / 2) + " life]";
            playerHUD.SetShield(playerUnit.shield);
            playerHUD.SetHP(playerUnit.HP);
            cross = false;
            return isDead;
        }
        else if (playerUnit.playerBoid > 0)
        {
            bool isDead = playerUnit.TakeDamage((int)(dmg * (1.0 - playerUnit.playerBoid)));
            dialogueText.text = "Because of your Boid card, " + opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)(dmg * (1.0 - playerUnit.playerBoid)) + " life]";
            playerHUD.SetShield(playerUnit.shield);
            playerHUD.SetHP(playerUnit.HP);
            playerUnit.playerBoid = 0;
            return isDead;
        }
        else
        {
            bool isDead = playerUnit.TakeDamage(dmg);
            dialogueText.text = opponentUnit.charName + "'s " + attackName + " hit you for [- " + (int)(dmg) + " life]";
            playerHUD.SetShield(playerUnit.shield);
            playerHUD.SetHP(playerUnit.HP);
            return isDead;
        }

    }

    public bool dealDamageBoid(string attackName, int dmg)
    {
        if (boid)
        {
            bool isDead = opponentUnit.TakeDamage((int)(dmg * 0.1));
            dialogueText.text = "Because of Jibb's Boid card, your " + attackName + " hit Jibb for [- " + (int)(dmg * 0.1) + " life]";
            opponentHUD.SetShield(opponentUnit.shield);
            opponentHUD.SetHP(opponentUnit.HP);
            boid = false;
            return isDead;
        }
        else
        {
            bool isDead = opponentUnit.TakeDamage(dmg);
            dialogueText.text = "Your " + attackName + " hit " + opponentUnit.charName + " for [- " + (int)(dmg) + " life]";
            opponentHUD.SetShield(opponentUnit.shield);
            opponentHUD.SetHP(opponentUnit.HP);
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
    }

    public void drawCardToggle()
    {
        if (playerUnit.mana >= 3 && state == BattleState.PLAYERTURN)
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
        if (state == BattleState.WON)
        {
            gameOverUI.gameOverScreen.SetActive(true);
            disableButtons();
            gameOverUI.LoadCard(playerUnit.relationship, opponentUnit.charName, playerUnit.deck);
            wonLost.text = "You defeated " + opponentUnit.charName + "!";
        }
        else if (state == BattleState.LOST)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
