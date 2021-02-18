using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    private SpriteRenderer rend;
    private Sprite gorbSprite, yorbSprite;

    // Start is called before the first frame update
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        gorbSprite = Resources.Load<Sprite>("gorb");
        yorbSprite = Resources.Load<Sprite>("yorb");
        rend.sprite = gorbSprite;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Sprite Switch");
            if(rend.sprite == gorbSprite)
                rend.sprite = yorbSprite;
            else if (rend.sprite == yorbSprite)
                rend.sprite = gorbSprite;
        }
    }
}
