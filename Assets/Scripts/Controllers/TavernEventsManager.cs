using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TavernEventsManager
{
    public static event UnityAction OnHeartRepaired;
    public static void HeartRepaired() => OnHeartRepaired?.Invoke();

    public static event UnityAction OnNightStarted;
    public static void NightStarted() => OnNightStarted?.Invoke();

    public static event UnityAction OnDayStarted;
    public static void DayStarted() => OnDayStarted?.Invoke();

    public static event UnityAction<int> OnCardsQuantityChanged;
    public static void CardsQuantityChanged(int newCardsQuantity) => OnCardsQuantityChanged?.Invoke(newCardsQuantity);

    public static event UnityAction<int> OnCoinsValueChanged;
    public static void CoinsValueChanged(int newCoinsValue) => OnCoinsValueChanged?.Invoke(newCoinsValue);

    public static event UnityAction<int> OnSoulsValueChanged;
    public static void SoulsValueChanged(int newSoulsValue) => OnSoulsValueChanged?.Invoke(newSoulsValue);

    public static event UnityAction<int> OnCoinsAdded;
    public static void CoinsAdded(int coinsValue) => OnCoinsAdded?.Invoke(coinsValue);

    public static event UnityAction<int> OnSoulsAdded;
    public static void SoulsAdded(int soulsValue) => OnSoulsAdded?.Invoke(soulsValue);

    public static event UnityAction<int> OnCoinsSubtracted;
    public static void CoinsSubtracted(int coinsValue) => OnCoinsSubtracted?.Invoke(coinsValue);

    public static event UnityAction<int> OnSoulsSubtracted;
    public static void SoulsSubtracted(int soulsValue) => OnSoulsSubtracted?.Invoke(soulsValue);

    public static event UnityAction<int> OnHappinessRateChanged;
    public static void HappinessRateChanged(int value) => OnHappinessRateChanged?.Invoke(value);

    public static event UnityAction<int> OnHappinessChanged;
    public static void HappinessChanged(int newHappinessValue) => OnHappinessChanged?.Invoke(newHappinessValue);
   
    //public static event UnityAction<VisitorAI> OnOneBeerGlassSold;
    //public static void OneBeerGlassSold(VisitorAI visitor)
    //{
    //    OnCoinsAdded?.Invoke(GameConfigManager.BeerSoldRewardInCoins);
    //    OnSoulsAdded?.Invoke(GameConfigManager.BeerSoldRewardInSouls);
    //    visitor.OnBeerDrinkEffect();
    //    OnOneBeerGlassSold?.Invoke(visitor);
    //}

    public static event UnityAction<VisitorAI> OnOneCarrotSold;
    public static void OneCarrotSold(VisitorAI visitor)
    {
        //OnCoinsAdded?.Invoke(GameConfigManager.BeerSoldRewardInCoins);
        //OnSoulsAdded?.Invoke(GameConfigManager.BeerSoldRewardInSouls);
        //visitor?.OnCarrotEatEffect();
        //OnOneCarrotSold?.Invoke(visitor);
    }

    public static event UnityAction<VisitorAI> OnVisitorTriedTakeCarrot;
    public static void VisitorTriedTakeCarrot(VisitorAI visitor) => OnVisitorTriedTakeCarrot?.Invoke(visitor);

    public static event UnityAction<VisitorAI> OnVisitorBecomeDefenderCard;
    public static void VisitorBecomeDefenderCard(VisitorAI defender) => OnVisitorBecomeDefenderCard?.Invoke(defender);

    public static event UnityAction<List<VisitorAI>> OnDefendersToCards;
    public static void DefendersToCards(List<VisitorAI> defenders) => OnDefendersToCards?.Invoke(defenders);

    public static event UnityAction<GameObject> OnVisitorLeftTavern;
    public static void VisitorLeftTavern(GameObject visiter) => OnVisitorLeftTavern?.Invoke(visiter);

    public static event UnityAction OnSwitchedToNightCanvas;
    public static void SwitchedToNightCanvas() => OnSwitchedToNightCanvas?.Invoke();

    public static event UnityAction OnSwitchedToDayCanvas;
    public static void SwitchedToDayCanvas() => OnSwitchedToDayCanvas?.Invoke();

    public static event UnityAction OnCameraSwitchedToCardGame;
    public static void CameraSwitchedToCardGame() => OnCameraSwitchedToCardGame?.Invoke();

    public static event UnityAction OnCameraSwitchedToNightAutoFight;
    public static void CameraSwitchedToNightAutoFight() => OnCameraSwitchedToNightAutoFight?.Invoke();

    public static event UnityAction OnSwitchedToNigthAutoFightCanvas;
    public static void SwitchedToNigthAutoFightCanvas() => OnSwitchedToNigthAutoFightCanvas?.Invoke();
    
    public static event UnityAction OnCameraSwitchedToFollowPlayer;
    public static void CameraSwitchedToFollowPlayer() => OnCameraSwitchedToFollowPlayer?.Invoke();

    public static event UnityAction OnCardsRendered;
    public static void CardsRendered() => OnCardsRendered?.Invoke();

    public static event UnityAction<PlayingCardView> OnPlayerChoseCard;
    public static void PlayerChoseCard(PlayingCardView card) => OnPlayerChoseCard?.Invoke(card);

    public static event UnityAction OnSpeedImproved;
    public static void SpeedImproved() => OnSpeedImproved?.Invoke();

    public static event UnityAction OnBeerIncomeImproved;
    public static void BeerIncomeImproved() => OnBeerIncomeImproved?.Invoke();

    public static event UnityAction OnHappinessImproved;
    public static void HappinessImproved() => OnHappinessImproved?.Invoke();

    public static event UnityAction OnNotEnoughtMessage;
    public static void NotEnoughtMessage() => OnNotEnoughtMessage?.Invoke();

    public static event UnityAction<UnityAction, int> OnTryToSpendCoins;
    public static void TryToSpendCoins(UnityAction successAction, int tryToSpendValue) => OnTryToSpendCoins?.Invoke(successAction, tryToSpendValue);

    public static event UnityAction<int, int, Guid> OnBeerOrdered;
    public static void BeerOrdered(int orderType, int secondsToCancelOrder, Guid orderId) => OnBeerOrdered?.Invoke(orderType, secondsToCancelOrder, orderId);

    public static event UnityAction<Guid> OnBeerOrderCanceled;
    public static void BeerOrderCanceled(Guid orderId) => OnBeerOrderCanceled?.Invoke(orderId);

}
