using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public Text nameText;
    public Slider hpSlider;
    public Slider shieldSlider;
    public Text manaNum;
    public Text hpNum;
    public Text shieldNum;

    public void SetHUD(Duelist duelist)
    {
        nameText.text = duelist.charName;
        hpSlider.maxValue = duelist.maxHP;
        shieldSlider.maxValue = duelist.maxShield;
        SetHP(duelist.HP);
        SetShield(duelist.shield);
        SetMana(duelist.mana);
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        hpNum.text = hp.ToString();
    }

    public void SetMana(int mana)
    {
        manaNum.text = mana.ToString();
    }

    public void SetShield(int shield)
    {
        shieldSlider.value = shield;
        shieldNum.text = shield.ToString();
    }
}
