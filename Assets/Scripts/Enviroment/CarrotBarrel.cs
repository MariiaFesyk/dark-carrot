using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarrotBarrel : PlayerInteractable
{
    [SerializeField] private TMP_Text carrotsQuantityText;
    [SerializeField] private TMP_Text barrelStatusText;
    [SerializeField] private Image warningSignImage;
    [SerializeField] private GameObject carrots;


    private int currentCarrotsQuantity;
    private bool isCarrotBarrelEmpty;

    private void OnEnable()
    {
        TavernEventsManager.OnVisitorTriedTakeCarrot += VisitorTriedTakeCarrotHandler;
        TavernEventsManager.OnDayStarted += DayStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnVisitorTriedTakeCarrot -= VisitorTriedTakeCarrotHandler;
        TavernEventsManager.OnDayStarted -= DayStartedHandler;
    }

    private void Start()
    {
        UpdateCarrotsText();
        warningSignImage.enabled = false;
        carrots.SetActive(false);
    }

    private void DayStartedHandler()
    {
        isCarrotBarrelEmpty = false;
    }

    private void VisitorTriedTakeCarrotHandler(VisitorAI visitor)
    {
        if (TryTakeCarrot())
        {
            TavernEventsManager.OneCarrotSold(visitor);
        }
    }

    public override void PlayerInteraction() => AddCarrots(GameConfigManager.CarrotsPlayerHolding);

    public void AddCarrots(int carrotsValue)
    {
        carrots.SetActive(true);
        currentCarrotsQuantity += carrotsValue;
        if (GameConfigManager.MaxCarrotsInBarrel < currentCarrotsQuantity)
        {
            currentCarrotsQuantity = GameConfigManager.MaxCarrotsInBarrel;
        }
        UpdateCarrotsText();
        HappinessHandler();
    }

    private bool TryTakeCarrot()
    {
        HappinessHandler();
        if (currentCarrotsQuantity > 0)
        {
            currentCarrotsQuantity--;
            UpdateCarrotsText();
            return true;
        }
        return false;
    }

    private void UpdateCarrotsText()
    {
        carrotsQuantityText.text = currentCarrotsQuantity.ToString();
        if(currentCarrotsQuantity == GameConfigManager.MaxCarrotsInBarrel)
        {
            barrelStatusText.text = "Full";
        }
        else if(currentCarrotsQuantity == 0)
        {
            barrelStatusText.text = "Empty";
        }
        else
        {
            barrelStatusText.text = "Have some";
        }
    }

    private void HappinessHandler()
    {
        bool wasCarrotBarrelEmpty = isCarrotBarrelEmpty;
        isCarrotBarrelEmpty = (currentCarrotsQuantity <= 0);
        if (wasCarrotBarrelEmpty != isCarrotBarrelEmpty)
        {
            int happinessRateChange = isCarrotBarrelEmpty ?
                -GameConfigManager.EmptyCarrotBarrelHappinessEffect : GameConfigManager.EmptyCarrotBarrelHappinessEffect;
            TavernEventsManager.HappinessRateChanged(happinessRateChange);
            warningSignImage.enabled = isCarrotBarrelEmpty;
            carrots.SetActive(!isCarrotBarrelEmpty);
        }
    }

}
