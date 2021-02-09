using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    /*     public virtual string title { get; set; }
        public virtual string desc { get; set; }
        public virtual Sprite sprite { get; set; }
        public virtual string type { get; set; } */

    public enum CardType { attack, defend, heal, talk };
    public CardType type;

    public string title;
    public string desc;
    public Sprite sprite;



    public abstract void play();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
