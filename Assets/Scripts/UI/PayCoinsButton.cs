using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayCoinsButton : MonoBehaviour
{
    public void PayCoins()
    {
        GameManager.UpdateGameState(GameManager.GameState.Day);
    }
}
