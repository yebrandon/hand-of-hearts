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
    // public Deck deck;
    public bool hasWon;

    public int damage;

    public bool TakeDamage(int dmg) {
        HP -= dmg;

        if (HP <= 0) {
            return true;
        }
        return false;
    }

}
