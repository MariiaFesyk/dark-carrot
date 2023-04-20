using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Config", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    [SerializeField] private int playerSpeed;
    
    [Header("Visitors")]
    [SerializeField] private int visitorSpeed;
    [SerializeField] private int visitorSecondsToLeave;
    [SerializeField] private int drinksToBecomeDefenderCard;
    [SerializeField] private int visitorsSpawnDelayMin;
    [SerializeField] private int visitorsSpawnDelayMax;
    [SerializeField] private int maxVisitersQuantity;
    [SerializeField] private int secondsToGetCarrot;
    [SerializeField] private int secondsBetweenOrders;
    [SerializeField] private int onBeerDrinkStrengthReward;
    [SerializeField] private int onCarrotEatStrengthReward;
    [SerializeField] private int visitorHappinessLevelToLeave;
    [SerializeField] private int firstVisiterSpawnDelay;
    [SerializeField] private int delayBetweenVisitersLeave;


    [Header("Carrot Barrel")]
    [SerializeField] private int carrotsPlayerHolding;
    [SerializeField] private int maxCarrotsInBarrel;

    [Header("Gramophone")]
    [SerializeField] private int decreaseRate;
    [SerializeField] private int decreaseStartDelay;
    [SerializeField] private int startVolumeLevel;

    [Header("Happiness")]
    [SerializeField] private int startHappinesLevel;
    [SerializeField] private int happinessMaxLevel;
    [SerializeField] private int dirtyTableHappinessEffect;
    [SerializeField] private int emptyCarrotBarrelHappinessEffect;
    [SerializeField] private int lowGramophoneVolumeHappinessEffect;
    [SerializeField] private int positiveHappinessEffect;

    [Header("Beer Producing")]
    [SerializeField] private int beerKegPriceInSouls; 
    [SerializeField] private int beerKegProducingTime;
    [SerializeField] private int fillingBeerGlassTime;

    [Header("Main Config")]
    [SerializeField] private int secondsToNightStarts;
    [SerializeField] private int beerSoldRewardInCoins;
    [SerializeField] private int beerSoldRewardInSouls;
    [SerializeField] private int carrotSoldRewardInSouls;
    [SerializeField] private int carrotSoldRewardInCoins;
    [SerializeField] private int startCoinsValue;
    [SerializeField] private int startSoulsValue;

    public int PlayerSpeed { get { return playerSpeed; } }
    public int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public int VisitorSpeed { get { return visitorSpeed; } }
    public int DrinksToBecomeDefenderCard { get { return drinksToBecomeDefenderCard; } }
    public int VisitorsSpawnDelayMin { get { return visitorsSpawnDelayMin; } }
    public int VisitorsSpawnDelayMax { get { return visitorsSpawnDelayMax; } }
    public int MaxVisitersQuantity { get { return maxVisitersQuantity; } }
    public int SecondsToGetCarrot { get { return secondsToGetCarrot; } }
    public int CarrotsPlayerHolding { get { return carrotsPlayerHolding; } }
    public int SecondsToNightStarts { get { return secondsToNightStarts; } }
    public int BeerSoldRewardInCoins { get { return beerSoldRewardInCoins; } }
    public int BeerSoldRewardInSouls { get { return beerSoldRewardInSouls; } }
    public int MaxCarrotsInBarrel { get { return maxCarrotsInBarrel; } }
    public int OnBeerDrinkStrengthReward { get { return onBeerDrinkStrengthReward; } }
    public int OnCarrotEatStrengthReward { get { return onCarrotEatStrengthReward; } }
    public int CarrotSoldRewardInSouls { get { return carrotSoldRewardInSouls; } }
    public int CarrotSoldRewardInCoins { get { return carrotSoldRewardInCoins; } }
    public int DirtyTableHappinessEffect { get { return dirtyTableHappinessEffect; } }
    public int EmptyCarrotBarrelHappinessEffect { get { return emptyCarrotBarrelHappinessEffect; } }
    public int LowGramophoneVolumeHappinessEffect { get { return lowGramophoneVolumeHappinessEffect; } }
    public int PositiveHappinessEffect { get { return positiveHappinessEffect; } }
    public int HappinessMaxLevel { get { return happinessMaxLevel; } }
    public int DecreaseRate { get { return decreaseRate; } }
    public int DecreaseStartDelay { get { return decreaseStartDelay; } }
    public int StartVolumeLevel { get { return startVolumeLevel; } }
    public int BeerKegPriceInSouls { get { return beerKegPriceInSouls; } }
    public int BeerKegProducingTime { get { return beerKegProducingTime; } }
    public int FillingBeerGlassTime { get { return fillingBeerGlassTime; } }
    public int StartHappinesLevel { get { return startHappinesLevel; } }
    public int StartCoinsValue { get { return startCoinsValue; } }
    public int StartSoulsValue { get { return startSoulsValue; } }
    public int VisitorHappinessLevelToLeave { get { return visitorHappinessLevelToLeave; } }
    public int FirstVisiterSpawnDelay { get { return firstVisiterSpawnDelay; } }
    public int DelayBetweenVisitersLeave { get { return delayBetweenVisitersLeave; } }
    public int SecondsBetweenOrders { get { return secondsBetweenOrders; } }


}
