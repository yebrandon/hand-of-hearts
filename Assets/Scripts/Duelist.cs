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
    public bool hasWon;

    public bool TakeDamage(int dmg)
    {
        if (shield != 0)
        {
            shield -= dmg;
        }
        else if (shield <= 0)
        {
            HP -= dmg;
        }

        if (HP <= 0)
        {
            return true;
        }
        return false;
    }
}


