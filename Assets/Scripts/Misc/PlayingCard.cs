using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    public int strength { get; set; }
    public int cardType { get; set; }
    public bool isUsed { get; set; }


    public PlayingCard(int strength, int cardType)
    {
        this.strength = strength;
        this.cardType = cardType;
        this.isUsed = false;
    }

   
}
