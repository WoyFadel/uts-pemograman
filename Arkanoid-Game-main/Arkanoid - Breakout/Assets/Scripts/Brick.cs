using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int points;
    public int hitsToBreak;
    public Sprite hitSprite;

    public void BreakBrick()
    {
        hitsToBreak--;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
    }
}
