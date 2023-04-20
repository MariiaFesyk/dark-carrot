using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private Transform playerDeckTransform;
    [SerializeField] private Transform enemyDeckTransform;
    [SerializeField] private Transform playerAttackDeckTransform;
    [SerializeField] private Transform enemyAttackDeckTransform;

    [SerializeField] private TMP_Text playerStrengthText;
    [SerializeField] private TMP_Text enemyStrengthText;

    [SerializeField] private PlayingCardView cardPrefab;

    private List<VisitorAI> defenders;

    private List<PlayingCardView> playersDeck = new List<PlayingCardView>();
    private List<PlayingCardView> enemysDeck = new List<PlayingCardView>();
    private Stack<PlayingCardView> playersAttackDeck = new Stack<PlayingCardView>();
    private Stack<PlayingCardView> enemysAttackDeck = new Stack<PlayingCardView>();

    private int enemyAttackStrength = 0;
    private int playerAttackStrength = 0;

    //private CardGameState currentCardGameState;

    private System.Random random = new System.Random();

    private void OnEnable()
    {
        TavernEventsManager.OnDefendersToCards += OnDefendersToCardsHandler;
        TavernEventsManager.OnCardsRendered += RenderAllCard;
        TavernEventsManager.OnPlayerChoseCard += PlayerChooseCardHandler;

        UpdateCardGameState(CardGameState.start);
    }

    private void OnDisable()
    {
        TavernEventsManager.OnDefendersToCards -= OnDefendersToCardsHandler;
        TavernEventsManager.OnCardsRendered -= RenderAllCard;
        TavernEventsManager.OnPlayerChoseCard += PlayerChooseCardHandler;
    }

    private void OnDefendersToCardsHandler(List<VisitorAI> defendersList)
    {
        defenders = defendersList;
    }

    private void RenderAllCard()
    {
        if (defenders != null)
        {
            //GeneratePlayersCards(defenders);
            UpdateCardGameState(CardGameState.start);
        } else
        {
            UpdateCardGameState(CardGameState.gameOver);
        }
    }

    //private void AddCardToDeck(VisitorType cardType, int cardStrength, Transform deck, bool isPlayersDeck)
    //{
    //    PlayingCardView cardView = Instantiate(cardPrefab, deck);
    //    cardView.Init(cardType, cardStrength);
    //    if (isPlayersDeck)
    //    {
    //        playersDeck.Add(cardView);
    //    } else
    //    {
    //        enemysDeck.Add(cardView);
    //    }
    //}

    //private void TestGeneratingCards()
    //{
    //    AddCardToDeck(VisitorType.VisitorType1, 10, playerDeckTransform,true);
    //    AddCardToDeck(VisitorType.VisitorType1, 15, playerDeckTransform, true);
    //    AddCardToDeck(VisitorType.VisitorType1, 5, playerDeckTransform, true);
    //    AddCardToDeck(VisitorType.VisitorType1, 7, playerDeckTransform, true);
    //    AddCardToDeck(VisitorType.VisitorType1, 10, enemyDeckTransform, false);
    //    AddCardToDeck(VisitorType.VisitorType2, 4, enemyDeckTransform, false);
    //    AddCardToDeck(VisitorType.VisitorType2, 9, enemyDeckTransform, false);
    //    AddCardToDeck(VisitorType.VisitorType2, 10, enemyDeckTransform, false);
    //}

    //private void GeneratePlayersCards(List<VisitorAI> defenders)
    //{
    //    foreach (VisitorAI d in defenders)
    //    {
    //        AddCardToDeck(d.CurrentType, d.Strength, playerDeckTransform, true);
    //        AddCardToDeck(d.CurrentType, d.Strength - 1, enemyDeckTransform, false);
    //    }
    //}
    
    private void CardGameStarts()
    {
        //TestGeneratingCards();
        if (playersDeck.Count > 0)
        {
            
            UpdateCardGameState(CardGameState.enemyTurn);
        }
    }

    private void EnemyTurn()
    {
            int randomIndex = random.Next(enemysDeck.Count);
            PlayingCardView card = enemysDeck[randomIndex];
            enemyAttackStrength += card.Strength;
            enemyStrengthText.text = enemyAttackStrength.ToString();
            card.transform.SetParent(enemyAttackDeckTransform);
            enemysDeck.Remove(card);
            enemysAttackDeck.Push(card);
            UpdateCardGameState(CardGameState.playerTurn);
    }

    private void PlayerTurn()
    {
        foreach (PlayingCardView c in playersDeck)
        {
            c.SetButtonEnebled(true);
        }
    }

    public void PlayerChooseCardHandler(PlayingCardView card)
    {
        foreach (PlayingCardView c in playersDeck)
        {
            c.SetButtonEnebled(false);
        }
        playerAttackStrength += card.Strength;
        playerStrengthText.text = playerAttackStrength.ToString();
        card.transform.SetParent(playerAttackDeckTransform.transform);
        playersDeck.Remove(card);
        playersAttackDeck.Push(card);
        UpdateCardGameState(CardGameState.fight);
    }

    private void CardFight()
    {
        PlayingCardView card;
        int res = playerAttackStrength - enemyAttackStrength;
        if(res >= 0)
        {
            enemyAttackStrength = 0;
            enemyStrengthText.text = enemyAttackStrength.ToString();
            playerAttackStrength = res;
            playerStrengthText.text = playerAttackStrength.ToString();
            enemysAttackDeck.Clear();
            foreach (Transform child in enemyAttackDeckTransform)
            {
                Destroy(child.gameObject);
            }
            if (playersAttackDeck.TryPop(out card))
            {
                card.SetStrength(res);
                playersAttackDeck.Clear();
                playersAttackDeck.Push(card);
            }
        }
        else
        {
            playerAttackStrength = 0;
            playerStrengthText.text = playerAttackStrength.ToString();
            enemyAttackStrength = -res;
            enemyStrengthText.text = enemyAttackStrength.ToString();
            playersAttackDeck.Clear();
            foreach (Transform child in playerAttackDeckTransform)
            {
                Destroy(child.gameObject);
            }
            if (enemysAttackDeck.TryPop(out card))
            {
                card.SetStrength(-res);
                enemysAttackDeck.Clear();
                enemysAttackDeck.Push(card);
            }
        }
        if((playersDeck.Count > 0 || playersAttackDeck.Count > 0) && (enemysDeck.Count > 0 || enemysAttackDeck.Count > 0))
        {
            UpdateCardGameState(CardGameState.enemyTurn);
        }else
        {
            UpdateCardGameState(CardGameState.gameOver);
        }
    }
  
    private void CardGameOver()
    {
        Debug.Log("Game Over");
        ClearAllDecks();
        GameManager.UpdateGameState(GameManager.GameState.Day);
    }

    private void ClearAllDecks()
    {
        playersDeck.Clear();
        playersAttackDeck.Clear();
        enemysDeck.Clear();
        enemysAttackDeck.Clear();
        foreach (Transform child in playerDeckTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerAttackDeckTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyAttackDeckTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyDeckTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateCardGameState(CardGameState newState)
    {
        //currentCardGameState = newState;
        switch (newState)
        {
            case CardGameState.day:
                break;
            case CardGameState.start:
                CardGameStarts();
                break;
            case CardGameState.enemyTurn:
                EnemyTurn();
                break;
            case CardGameState.playerTurn:
                PlayerTurn();
                break;
            case CardGameState.fight:
                CardFight();
                break;
            case CardGameState.gameOver:
                CardGameOver();
                break;
            default:
                break;
        }

    }

}

public enum CardGameState{
    day,
    start,
    enemyTurn,
    playerTurn,
    fight,
    gameOver
}

