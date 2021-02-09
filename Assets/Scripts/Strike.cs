using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : Card
{
    public Strike()
    {
        type = CardType.attack;
        title = "Strike";
        desc = "Deals 10 damage to your opponent";
        // sprite = some sprite
        //color?
    }


    public override void play()
    {
        //call on button press?
        Debug.Log(title);
        Debug.Log(type.ToString());
        // subtract 10 hp from enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
