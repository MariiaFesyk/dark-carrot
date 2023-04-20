using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFlag : MonoBehaviour
{
    [SerializeField] private Image orderIcon;
    [SerializeField] private Image orderTimer;
    private Coroutine orderTimerCoroutine;
    private Guid orderId;

    public Guid OrderId
    {
        get => orderId;
        set => orderId = value;
    }

    public void renderFlag(Sprite orderIcon, int timerSeconds)
    {
        this.orderIcon.sprite = orderIcon;
        orderTimer.fillAmount = 0;
        if (timerSeconds > 0)
        {
            orderTimerCoroutine = StartCoroutine(OrderTimerCoroutine(timerSeconds));
        }
    }
   

    IEnumerator OrderTimerCoroutine(int seconds)
    {
        float time = seconds;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            orderTimer.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if(orderTimerCoroutine != null)
        {
            StopCoroutine(orderTimerCoroutine);
        }
    }

}
