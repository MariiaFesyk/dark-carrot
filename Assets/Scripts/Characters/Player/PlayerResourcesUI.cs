using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text soulsText;
    [SerializeField] private Image coinsImage;
    [SerializeField] private Image soulsImage;
    [SerializeField] private Transform coinsUI;
    [SerializeField] private Transform soulsUI;

    private float uiShowDuration = 1f;
    private Vector3 uiMoveVector3 = new Vector3(0f, 0.005f, 0f);

    private void OnEnable()
    {
        TavernEventsManager.OnCoinsAdded += OnCoinsAddedHandler;
        TavernEventsManager.OnSoulsAdded += OnSoulsAddedHandler;
        TavernEventsManager.OnCoinsSubtracted += OnCoinsSubtractionHandler;
        TavernEventsManager.OnSoulsSubtracted += OnSoulsSubtractionHandler;

    }

    private void OnDisable()
    {
        TavernEventsManager.OnCoinsAdded -= OnCoinsAddedHandler;
        TavernEventsManager.OnSoulsAdded -= OnSoulsAddedHandler;
        TavernEventsManager.OnCoinsSubtracted -= OnCoinsSubtractionHandler;
        TavernEventsManager.OnSoulsSubtracted -= OnSoulsSubtractionHandler;


    }

    private void Start()
    {
        coinsText.enabled = false;
        soulsText.enabled = false;
        coinsImage.enabled = false;
        soulsImage.enabled = false;
    }

    private void OnCoinsSubtractionHandler(int value)
    {
        coinsText.text = "-" + value;
        StartCoroutine(ShowSpendUICoroutine(coinsText, coinsImage, coinsUI));
    }

    private void OnSoulsSubtractionHandler(int value)
    {
        soulsText.text = "-" + value;
        StartCoroutine(ShowSpendUICoroutine(soulsText, soulsImage, soulsUI));
    }

    private void OnCoinsAddedHandler(int value)
    {
        coinsText.text = "+" + value;
        StartCoroutine(ShowSpendUICoroutine(coinsText, coinsImage, coinsUI));
    }

    private void OnSoulsAddedHandler(int value)
    {
        soulsText.text = "+" + value;
        StartCoroutine(ShowSpendUICoroutine(soulsText, soulsImage, soulsUI));
    }

    private IEnumerator ShowSpendUICoroutine(TMP_Text valueText, Image resourceIcon, Transform resourceUI)
    {
        float time = uiShowDuration;
        float elapsedTime = 0f;
        valueText.enabled = true;
        resourceIcon.enabled = true;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            resourceUI.Translate(uiMoveVector3);
            yield return null;
        }
        valueText.enabled = false;
        resourceIcon.enabled = false;
    }

   
}