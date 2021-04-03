using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duelist : MonoBehaviour
{
    public string charName;
    public int HP;
    public int maxHP;
    public int shield;
    public int maxShield;
    public int mana;
    public int maxMana;
    public bool hasWon;
    public int damage;

    // Deals damage and returns a boolean representing if the duelist has been defeated or not
    public bool TakeDamage(int dmg)
    {
        if (shield > 0)
        { // if the shield has not been depleted previously
            shield -= dmg; // subtract damage from the shield
            if (shield < 0) // if the attack depleted the shield
            {
                HP += shield; // subtract remaining damage from HP
                if (HP < 0)
                    HP = 0;
                shield = 0;
            }
        }
        else if (shield <= 0)
        { // if the shield has been depleted
            shield = 0; // reset shield to 0
            HP -= dmg; // subtract damage from the hp
            if (HP < 0)
                HP = 0;
        }

        if (HP <= 0)
        { // if the hp is less than or equal to 0
            return true; // return true, the duelist is defeated
        }
        return false; // return false, the duelist is not defeated
    }

    // INcreases the user's shield
    public void Guard(int amnt)
    {
        if ((shield + amnt) >= maxShield)
        { // if the heal will be equal to or exceed the max
            shield = maxShield; // set shield to the max
        }
        else
        { // otherwise, heal the amount
            shield += amnt;
        }
    }

    // Heals the user's hp
    public void Recover(int amnt)
    {
        if ((HP + amnt) >= maxHP)
        { // if the heal will be equal or exceed the max
            HP = maxHP; // set HP to the max
        }
        else
        { // otherwise, heal the amount
            HP += amnt;
        }
    }
}


