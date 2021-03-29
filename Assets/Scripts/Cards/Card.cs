using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { OFFENSIVE, DEFENSIVE, TALK };

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public Sprite artwork;
    public CardType type; 
    public int cost;
    public int effect;
}
