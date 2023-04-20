using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
    public static GameConfigManager Instance;

    public GameConfig[] presets;
    public  int selectedPresetIndex = 0;

    private static int playerSpeed;
    private static int visitorSpeed;
    private static int visitorSecondsToLeave;
    private static int secondsToNightStarts;
    private static int beerSoldRewardInCoins;
    private static int beerSoldRewardInSouls;
    private static int drinksToBecomeDefenderCard;
    private static int visitorsSpawnDelayMin;
    private static int visitorsSpawnDelayMax;
    private static int maxVisitersQuantity;
    private static int secondsToGetCarrot;
    private static int carrotsPlayerHolding;
    private static int maxCarrotsInBarrel;
    private static int onBeerDrinkStrengthReward;
    private static int onCarrotEatStrengthReward;
    private static int carrotSoldRewardInSouls;
    private static int carrotSoldRewardInCoins;
    private static int dirtyTableHappinessEffect;
    private static int emptyCarrotBarrelHappinessEffect;
    private static int lowGramophoneVolumeHappinessEffect;
    private static int positiveHappinessEffect;
    private static int happinessMaxLevel;
    private static int decreaseRate;
    private static int decreaseStartDelay;
    private static int startVolumeLevel;
    private static int beerKegPriceInSouls;
    private static int beerKegProducingTime;
    private static int fillingBeerGlassTime;
    private static int startHappinesLevel;
    private static int startCoinsValue;
    private static int startSoulsValue;
    private static int visitorHappinessLevelToLeave;
    private static int firstVisiterSpawnDelay;
    private static int delayBetweenVisitersLeave;
    private static int secondsBetweenOrders;



    public static int PlayerSpeed { get { return playerSpeed; } }
    public static int VisitorSecondsToLeave { get { return visitorSecondsToLeave; } }
    public static int VisitorSpeed { get { return visitorSpeed; } }
    public static int SecondsToNightStarts { get { return secondsToNightStarts; } }
    public static int BeerSoldRewardInCoins { get { return beerSoldRewardInCoins; } }
    public static int BeerSoldRewardInSouls { get { return beerSoldRewardInSouls; } }
    public static int DrinksToBecomeDefenderCard { get { return drinksToBecomeDefenderCard; } }
    public static int VisitorsSpawnDelayMin { get { return visitorsSpawnDelayMin; } }
    public static int VisitorsSpawnDelayMax { get { return visitorsSpawnDelayMax; } }
    public static int MaxVisitersQuantity { get { return maxVisitersQuantity; } }
    public static int CarrotsPlayerHolding { get { return carrotsPlayerHolding; } }
    public static int SecondsToGetCarrot { get { return secondsToGetCarrot; } }
    public static int MaxCarrotsInBarrel { get { return maxCarrotsInBarrel; } }
    public static int OnBeerDrinkStrengthReward { get { return onBeerDrinkStrengthReward; } }
    public static int OnCarrotEatStrengthReward { get { return onCarrotEatStrengthReward; } }
    public static int CarrotSoldRewardInSouls { get { return carrotSoldRewardInSouls; } }
    public static int CarrotSoldRewardInCoins { get { return carrotSoldRewardInCoins; } }
    public static int DirtyTableHappinessEffect { get { return dirtyTableHappinessEffect; } }
    public static int EmptyCarrotBarrelHappinessEffect { get { return emptyCarrotBarrelHappinessEffect; } }
    public static int LowGramophoneVolumeHappinessEffect { get { return lowGramophoneVolumeHappinessEffect; } }
    public static int PositiveHappinessEffect { get { return positiveHappinessEffect; } }
    public static int HappinessMaxLevel { get { return happinessMaxLevel; } }
    public static int DecreaseRate { get { return decreaseRate; } }
    public static int DecreaseStartDelay { get { return decreaseStartDelay; } }
    public static int StartVolumeLevel { get { return startVolumeLevel; } }
    public static int BeerKegPriceInSouls { get { return beerKegPriceInSouls; } }
    public static int BeerKegProducingTime { get { return beerKegProducingTime; } }
    public static int FillingBeerGlassTime { get { return fillingBeerGlassTime; } }
    public static int StartHappinesLevel { get { return startHappinesLevel; } }
    public static int StartCoinsValue { get { return startCoinsValue; } }
    public static int StartSoulsValue { get { return startSoulsValue; } }
    public static int VisitorHappinessLevelToLeave { get { return visitorHappinessLevelToLeave; } }
    public static int FirstVisiterSpawnDelay { get { return firstVisiterSpawnDelay; } }
    public static int DelayBetweenVisitersLeave { get { return delayBetweenVisitersLeave; } }
    public static int SecondsBetweenOrders { get { return secondsBetweenOrders; } }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        CurrentGameConfigHandler();
    }


    private void CurrentGameConfigHandler()
    {
        if (selectedPresetIndex >= 0 && selectedPresetIndex < presets.Length)
        {
            playerSpeed = presets[selectedPresetIndex].PlayerSpeed;
            visitorSpeed = presets[selectedPresetIndex].VisitorSpeed;
            visitorSecondsToLeave = presets[selectedPresetIndex].VisitorSecondsToLeave;
            secondsToNightStarts = presets[selectedPresetIndex].SecondsToNightStarts;
            beerSoldRewardInCoins = presets[selectedPresetIndex].BeerSoldRewardInCoins;
            beerSoldRewardInSouls = presets[selectedPresetIndex].BeerSoldRewardInSouls;
            drinksToBecomeDefenderCard = presets[selectedPresetIndex].DrinksToBecomeDefenderCard;
            maxVisitersQuantity = presets[selectedPresetIndex].MaxVisitersQuantity;
            visitorsSpawnDelayMin = presets[selectedPresetIndex].VisitorsSpawnDelayMin;
            visitorsSpawnDelayMax = presets[selectedPresetIndex].VisitorsSpawnDelayMax;
            secondsToGetCarrot = presets[selectedPresetIndex].SecondsToGetCarrot;
            carrotsPlayerHolding = presets[selectedPresetIndex].CarrotsPlayerHolding;
            maxCarrotsInBarrel = presets[selectedPresetIndex].MaxCarrotsInBarrel;
            onBeerDrinkStrengthReward = presets[selectedPresetIndex].OnBeerDrinkStrengthReward;
            onCarrotEatStrengthReward = presets[selectedPresetIndex].OnCarrotEatStrengthReward;
            carrotSoldRewardInSouls = presets[selectedPresetIndex].CarrotSoldRewardInSouls;
            carrotSoldRewardInCoins = presets[selectedPresetIndex].CarrotSoldRewardInCoins;
            dirtyTableHappinessEffect = presets[selectedPresetIndex].DirtyTableHappinessEffect;
            emptyCarrotBarrelHappinessEffect = presets[selectedPresetIndex].EmptyCarrotBarrelHappinessEffect;
            lowGramophoneVolumeHappinessEffect = presets[selectedPresetIndex].LowGramophoneVolumeHappinessEffect;
            positiveHappinessEffect = presets[selectedPresetIndex].PositiveHappinessEffect;
            happinessMaxLevel = presets[selectedPresetIndex].HappinessMaxLevel;
            decreaseRate = presets[selectedPresetIndex].DecreaseRate;
            decreaseStartDelay = presets[selectedPresetIndex].DecreaseStartDelay;
            startVolumeLevel = presets[selectedPresetIndex].StartVolumeLevel;
            beerKegPriceInSouls = presets[selectedPresetIndex].BeerKegPriceInSouls;
            beerKegProducingTime = presets[selectedPresetIndex].BeerKegProducingTime;
            fillingBeerGlassTime = presets[selectedPresetIndex].FillingBeerGlassTime;
            startHappinesLevel = presets[selectedPresetIndex].StartHappinesLevel;
            startCoinsValue = presets[selectedPresetIndex].StartCoinsValue;
            startSoulsValue = presets[selectedPresetIndex].StartSoulsValue;
            visitorHappinessLevelToLeave = presets[selectedPresetIndex].VisitorHappinessLevelToLeave;
            firstVisiterSpawnDelay = presets[selectedPresetIndex].FirstVisiterSpawnDelay;
            delayBetweenVisitersLeave = presets[selectedPresetIndex].DelayBetweenVisitersLeave;
            secondsBetweenOrders = presets[selectedPresetIndex].SecondsBetweenOrders;
        }
        else
        {
            Debug.LogError("Invalid preset index!");
        }
    }
}



