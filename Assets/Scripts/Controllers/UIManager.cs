using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dayCanvas;
    [SerializeField] private TMP_Text coinsValueText;
    [SerializeField] private TMP_Text soulsValueText;
    [SerializeField] private Image nigthTimerIndicator;
    [SerializeField] private Image happinesIconImage;
    [SerializeField] private Sprite happinessGoodSprite;
    [SerializeField] private Sprite happinessNormalSprite;
    [SerializeField] private Sprite happinessBadSprite;
    [SerializeField] private Slider happinessSlider;
    [Header("Improvements")]
    [SerializeField] private GameObject improvementsGameObject;
    [SerializeField] private RectTransform improvementsList;
    [SerializeField] private Image improvementsBackground;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject bell;
    [Header("Improvements variables")]
    [SerializeField] private float improvementsBackgroundTintTimeSeconds = 2;
    [SerializeField] private float improvementsSecondsToShow = 2;

    private void Start()
    {
        happinesIconImage.sprite = happinessNormalSprite;
        happinessSlider.minValue = -GameConfigManager.HappinessMaxLevel;
        happinessSlider.maxValue = GameConfigManager.HappinessMaxLevel;
        HappinessChangeHandler(GameConfigManager.StartHappinesLevel);
    }
    
    private void OnEnable()
    {
        TavernEventsManager.OnSwitchedToDayCanvas += SwitchToDayCanvas;
        TavernEventsManager.OnSwitchedToNightCanvas += SwitchToNightCanvas;
        TavernEventsManager.OnCoinsValueChanged += CoinsValueChangedHandler;
        TavernEventsManager.OnSoulsValueChanged += SoulsValueChangedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessChangeHandler;
        TavernEventsManager.OnHeartRepaired += NightTimerHandler;
        TavernEventsManager.OnSwitchedToNigthAutoFightCanvas += OnSwitchedToNigthAutoFightCanvasHandler;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnSwitchedToDayCanvas -= SwitchToDayCanvas;
        TavernEventsManager.OnSwitchedToNightCanvas -= SwitchToNightCanvas;
        TavernEventsManager.OnCoinsValueChanged -= CoinsValueChangedHandler;
        TavernEventsManager.OnSoulsValueChanged -= SoulsValueChangedHandler;
        TavernEventsManager.OnHappinessChanged += HappinessChangeHandler;
        TavernEventsManager.OnHeartRepaired -= NightTimerHandler;
        TavernEventsManager.OnSwitchedToNigthAutoFightCanvas -= OnSwitchedToNigthAutoFightCanvasHandler;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
    }

    private void CoinsValueChangedHandler(int newValue)
    {
        coinsValueText.text = newValue.ToString();
    }   
    
    private void SoulsValueChangedHandler(int newValue) => soulsValueText.text = newValue.ToString();
    
    private void HappinessChangeHandler(int currentHappinessValue)
    {
        happinessSlider.value = currentHappinessValue;
        int threshold = GameConfigManager.HappinessMaxLevel / 2;
        switch (currentHappinessValue)
        {
            case int happiness when currentHappinessValue < -threshold:
                happinesIconImage.sprite = happinessBadSprite;
                break;
            case int happiness when currentHappinessValue > -threshold && currentHappinessValue < threshold:
                happinesIconImage.sprite = happinessNormalSprite;
                break;
            case int happiness when currentHappinessValue > threshold:
                happinesIconImage.sprite = happinessGoodSprite;
                break;
            default:
                break;
        }
    }

    private void OnSwitchedToNigthAutoFightCanvasHandler()
    {
        //dayCanvas.SetActive(false);
        //nightCanvas.SetActive(false);
        improvementsGameObject.SetActive(true);
    }

    private void SwitchToDayCanvas()
    {
        dayCanvas.SetActive(true);
        //nightCanvas.SetActive(false);
        improvementsGameObject.SetActive(false);
    }

    private void SwitchToNightCanvas()
    {
        dayCanvas.SetActive(false);
        //nightCanvas.SetActive(true);
        improvementsGameObject.SetActive(false);
    }

    private void NightTimerHandler() => StartCoroutine(NightTimerCoroutine());
    
    private IEnumerator NightTimerCoroutine()
    {
        float time = GameConfigManager.SecondsToNightStarts;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            nigthTimerIndicator.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            yield return null;
        }
    }

    private void NightStartedHandler()
    {
        improvementsGameObject.SetActive(true);
        nextButton.SetActive(false);
        improvementsList.localPosition = new Vector3(0f, 1100f, 0f);
        StartCoroutine(ImprovementsBackgroundCoroutine());
    }

    private IEnumerator ImprovementsBackgroundCoroutine()
    {
        float time = improvementsSecondsToShow;
        float elapsedTime = 0f;
        Color tempColor;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            tempColor = improvementsBackground.color;
            tempColor.a = Mathf.Lerp(0f, .65f, elapsedTime / time);
            improvementsBackground.color = tempColor;
            yield return null;
        }
        StartCoroutine(ImprovementsWindowAnimation());
    }

    private IEnumerator ImprovementsWindowAnimation()
    {
        float time = improvementsSecondsToShow;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var yPos = Mathf.Lerp(1100f, 0f, elapsedTime / time);
            improvementsList.localPosition = new Vector3(0f, yPos, 0f);
            yield return null;
        }
        nextButton.SetActive(true);
    }
}
