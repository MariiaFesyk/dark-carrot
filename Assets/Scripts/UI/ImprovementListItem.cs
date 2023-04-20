using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImprovementListItem : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemEffectText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Button buyButton;

    private bool isApplied;
    private int price;

    private UnityEvent improvementEffectEvent;
    private UnityEvent onClickEvent;


    public void RenderListItem(Sprite icon, string itemName, string itemEffect,
        string itemDesc, int itemPrice, UnityEvent effectEvent)
    {
        itemIcon.sprite = icon;
        itemNameText.text = itemName;
        itemEffectText.text = itemEffect;
        itemDescriptionText.text = itemDesc;
        itemPriceText.text = itemPrice.ToString();
        price = itemPrice;
        improvementEffectEvent = effectEvent;
        
    }

    private void OnEnable()
    {
        buyButton.onClick.AddListener(TryToBuyImprovementHandler);
    }

    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(TryToBuyImprovementHandler);
    }

    private void OnBuySuccessHandler()
    {
        isApplied = true;
        buyButton.GetComponent<Image>().color = Color.gray;
        buyButton.enabled = false;
        onClickEvent?.Invoke();
    }

    private void TryToBuyImprovementHandler()
    {
        TavernEventsManager.TryToSpendCoins(OnBuySuccessHandler, price);
       
    }
}
