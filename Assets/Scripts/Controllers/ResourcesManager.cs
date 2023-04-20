using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private TMP_Text happinessText;
    [SerializeField] private TMP_Text happinessRateText;

    private int coins;
    private int souls;
    private int happiness;
    private int happinessRate;
    private int happinessImprovementBonus;
    private List<VisitorAI> defendersCards = new List<VisitorAI>();
    private Coroutine updateHappinessCoroutine;
    private AudioManager audioManager;

    private void OnEnable()
    {
        TavernEventsManager.OnCoinsAdded += AddCoins;
        TavernEventsManager.OnSoulsAdded += AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard += VisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnHappinessRateChanged += HappinessRateChangedHandler;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
        TavernEventsManager.OnHeartRepaired += DayStartedHandler;
        TavernEventsManager.OnHappinessImproved += HappinessImprovedHandler;
        TavernEventsManager.OnTryToSpendCoins += TryToSpendCoinsHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnCoinsAdded -= AddCoins;
        TavernEventsManager.OnSoulsAdded -= AddSouls;
        TavernEventsManager.OnVisitorBecomeDefenderCard -= VisitorBecomeDefenderCardHandler;
        TavernEventsManager.OnHappinessRateChanged -= HappinessRateChangedHandler;
        TavernEventsManager.OnNightStarted -= NightStartedHandler;
        TavernEventsManager.OnHeartRepaired -= DayStartedHandler;
        TavernEventsManager.OnHappinessImproved -= HappinessImprovedHandler;
        TavernEventsManager.OnTryToSpendCoins -= TryToSpendCoinsHandler;
    }

    private void Start()
    {
        coins = GameConfigManager.StartCoinsValue;
        souls = GameConfigManager.StartSoulsValue;
        happiness = GameConfigManager.StartHappinesLevel;
        TavernEventsManager.CoinsValueChanged(coins);
        TavernEventsManager.SoulsValueChanged(souls);
        //updateHappinessCoroutine = StartCoroutine(UpdateHappiness());
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void AddCoins(int value)
    {
        coins += value;
        TavernEventsManager.CoinsValueChanged(coins);
        audioManager.CoinsAddedSoundPlay();
    }

    public bool TrySpendCoins(int value)
    {
        if(value <= coins)
        {
            coins -= value;
            TavernEventsManager.CoinsValueChanged(coins);
            TavernEventsManager.CoinsSubtracted(value);
            return true;
        }
        return false;
    }

    private void TryToSpendCoinsHandler(UnityAction successAction, int tryToSpendValue)
    {
        if (TrySpendCoins(tryToSpendValue))
        {
            successAction?.Invoke();
        }
    }

    private void AddSouls(int value)
    {
        souls += value;
        TavernEventsManager.SoulsValueChanged(souls);
    }

    public bool TrySpendSouls(int value)
    {
        if(value <= souls)
        {
            souls -= value;
            TavernEventsManager.SoulsValueChanged(souls);
            TavernEventsManager.SoulsSubtracted(value);
            return true;
        }
        return false;
    }

    private void VisitorBecomeDefenderCardHandler(VisitorAI visitor)
    {
        defendersCards.Add(visitor);
        TavernEventsManager.CardsQuantityChanged(defendersCards.Count);
    }
    private void NightStartedHandler()
    {
        StopCoroutine(updateHappinessCoroutine);
        happiness = happinessRate = 0;
        if(defendersCards.Count > 0)
        {
            TavernEventsManager.DefendersToCards(defendersCards);
        }
    }

    private void DayStartedHandler()
    {
        updateHappinessCoroutine = StartCoroutine(UpdateHappiness());
    }

    private IEnumerator UpdateHappiness()
    {
        while (true)
        {
            if (happinessRate == 0)
            {
                happiness += GameConfigManager.PositiveHappinessEffect;
            }
            if (happiness > GameConfigManager.HappinessMaxLevel)
            {
                happiness = GameConfigManager.HappinessMaxLevel;
            }
            if (happiness < -GameConfigManager.HappinessMaxLevel)
            {
                happiness = -GameConfigManager.HappinessMaxLevel;
            }
            happiness += happinessRate+happinessImprovementBonus;
            TavernEventsManager.HappinessChanged(happiness);
            UpdateText();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateText()
    {
        happinessText.text = happiness.ToString();
        happinessRateText.text = happinessRate.ToString();
    }

    private void HappinessRateChangedHandler(int value)
    {
        happinessRate += value;
    }
    private void HappinessImprovedHandler()
    {
        happinessImprovementBonus++;
    }
}
