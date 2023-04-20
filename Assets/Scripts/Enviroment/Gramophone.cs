using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gramophone : PlayerInteractable
{
    [SerializeField] private TMP_Text gramophoneVolumeText;
    [SerializeField] private Image warningSignImage;
    [SerializeField] private Image volumeIcon;

    private int maxVolume = 100;
    private int currentVolume;
    private bool isVolumeLow;
    private int volumeThreshold = 50;
    private Coroutine volumeDecreaseCoroutine;

    private void OnEnable()
    {
        TavernEventsManager.OnHeartRepaired += HeartRepairedHander;
        TavernEventsManager.OnNightStarted += NightStartedHandler;
    }

    private void OnDisable()
    {
        TavernEventsManager.OnHeartRepaired -= HeartRepairedHander;
        TavernEventsManager.OnNightStarted -= NightStartedHandler;
    }

    private void Start()
    {
        currentVolume = GameConfigManager.StartVolumeLevel;
        gramophoneVolumeText.text = currentVolume.ToString();
        warningSignImage.enabled = false;
        isInteractable = false;
        
    }

    private void HeartRepairedHander()
    {
        isInteractable = true;
        volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
    }

    private void NightStartedHandler()
    {
        isInteractable = false;
    }

    public override void PlayerInteraction()
    {
        if(currentVolume <= 0)
        {
            volumeDecreaseCoroutine = StartCoroutine(VolumeDecreaseCoroutine());
        }
        currentVolume = maxVolume;
        gramophoneVolumeText.text = currentVolume.ToString();
        HappinessHandler();
        VolumeIconHandler();
    }

    IEnumerator VolumeDecreaseCoroutine()
    {
        yield return new WaitForSeconds(GameConfigManager.DecreaseStartDelay);

        while(currentVolume > 0)
        {
            yield return new WaitForSeconds(1);
            currentVolume = currentVolume - GameConfigManager.DecreaseRate;
            if (currentVolume < 0)
            {
                currentVolume = 0;
            }
            gramophoneVolumeText.text = currentVolume.ToString();
            HappinessHandler();
            VolumeIconHandler();
        }
    }

    private void VolumeIconHandler()
    {
        if(currentVolume >= 95)
        {
            volumeIcon.fillAmount = 0;
        }
        if(currentVolume < 95 && currentVolume >= 45)
        {
            volumeIcon.fillAmount = 0.38f;
        }
        else if(currentVolume<45 && currentVolume > 5)
        {
            volumeIcon.fillAmount = 0.7f;
        }
        else if(currentVolume < 5)
        {
            volumeIcon.fillAmount = 1;
        }
    }
    
    private void HappinessHandler()
    {
        bool wasVolumeLow = isVolumeLow;
        isVolumeLow = (currentVolume <= volumeThreshold);

        if (wasVolumeLow != isVolumeLow)
        {
            int happinessRateChange = isVolumeLow ?
                -GameConfigManager.LowGramophoneVolumeHappinessEffect : GameConfigManager.LowGramophoneVolumeHappinessEffect;
            TavernEventsManager.HappinessRateChanged(happinessRateChange);
            warningSignImage.enabled = isVolumeLow;
        }
    }
}
