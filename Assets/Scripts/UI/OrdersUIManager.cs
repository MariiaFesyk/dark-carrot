using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUIManager : MonoBehaviour
{
    [SerializeField] private Sprite redBeerSprite;
    [SerializeField] private Sprite greenBeerSprite;
    [SerializeField] private OrderFlag template;
    [SerializeField] private Transform ordersBar;

    private List<OrderFlag> activeFlags = new List<OrderFlag>();

    private void OnEnable()
    {
        TavernEventsManager.OnBeerOrdered += OnBeerOrderedHandler;
        TavernEventsManager.OnBeerOrderCanceled += OnBeerOrderCanceledHandler;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnBeerOrdered -= OnBeerOrderedHandler;
        TavernEventsManager.OnBeerOrderCanceled -= OnBeerOrderCanceledHandler;
        TavernEventsManager.OnNightStarted -= NightStartedHandler;
    }
    

    private void OnBeerOrderedHandler(int orderType, int secondsToCancelOrder, Guid orderId)
    {
        OrderFlag order = Instantiate(template, ordersBar);
        activeFlags.Add(order);
        order.OrderId = orderId;
        order.renderFlag(GetOrderSprite(orderType), secondsToCancelOrder);
    }

    private void OnBeerOrderCanceledHandler(Guid orderId)
    {
        OrderFlag order = activeFlags.Find(o => o.OrderId == orderId);
        if(order != null)
        {
            activeFlags.Remove(order);
            Destroy(order.gameObject);
        }
    }

    private Sprite GetOrderSprite(int orderType)
    {
        switch (orderType)
        {
            case 1:
                return greenBeerSprite;
            case 2:
                return redBeerSprite;
            default:
                return redBeerSprite;
        }
    }

    private void NightStartedHandler()
    {
        foreach (OrderFlag o in activeFlags)
        {
            Destroy(o.gameObject);
        }
        activeFlags.Clear();
    }
}
