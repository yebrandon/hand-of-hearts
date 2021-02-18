﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public Text nameText;
    public Slider hpSlider;
    public Slider shieldSlider;

    public void SetHUD(Duelist duelist) {
        nameText.text = duelist.charName;
        hpSlider.maxValue = duelist.maxHP;
        shieldSlider.maxValue = duelist.maxShield;
        SetHP(duelist.HP);
        SetShield(duelist.shield);
    }

    public void SetHP(int hp) {
        hpSlider.value = hp;
    }

    public void SetShield(int shield) {
        shieldSlider.value = shield;
    }
}
