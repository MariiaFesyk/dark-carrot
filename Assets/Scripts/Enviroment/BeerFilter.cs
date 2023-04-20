using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeerFilter : PlayerInteractable
{
    //[SerializeField] private GameObject kegOfBeer;
    [SerializeField] private Image beerProducingIndicator;
    [SerializeField] private Image beerTypeImage;
    [SerializeField] private Sprite redBeerGlassSprite;
    [SerializeField] private Sprite greenBeerGlassSprite;

    private bool isFillingGlass = false;
    private int beerGlasses;
    private Coroutine fillingGlass;
    private int currentBeerType;
    private int currentPlayerSpeed;


    private void Start()
    {
        //kegOfBeer.SetActive(false);
        beerTypeImage.enabled = false;
        beerProducingIndicator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if ((beerGlasses > 0) || player.isHoldingBeerKeg)
            {
                player.SetTarget(gameObject);
                outline.OutlineWidth = 2.5f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetTarget(player.gameObject);
            if (fillingGlass != null && isFillingGlass)
            {
                StopCoroutine(fillingGlass);
                isFillingGlass = false;
                beerProducingIndicator.enabled = false;
                Debug.Log("FillingGlass fail");
            }
            outline.OutlineWidth = 0;
        }
    }

    public override void PlayerInteraction()
    {
        if (player.isHoldingCleaningMaterials)
        {
            return;
        }
        if (player.isHoldingBeerKeg)
        {
            SetBeerKeg(player.CurrentBeerType);
        }
        else if (beerGlasses > 0 && !player.isHoldingGlassOfBeer)
        {
            TryGiveGlassToPlayer();
        }
    }

    private void SetBeerKeg(int beerType)
    {
        if (currentBeerType == beerType)
        {
            beerGlasses += 4;
        } else
        {
            currentBeerType = beerType;
            beerGlasses = 4;
        }
        player.ReleaseBeerKeg();
        //kegOfBeer.SetActive(true);
        UpdadeBeerTypeSprite();
        beerTypeImage.enabled = true;
    }

    private void TryGiveGlassToPlayer()
    {
        if (!isFillingGlass)
        {
            if (--beerGlasses <= 0)
            {
                //kegOfBeer.SetActive(false);
                beerTypeImage.enabled = false;
            }
            fillingGlass = StartCoroutine(FillingGlassCroutine());
        } else
        {
            Debug.Log("You can not start filling glass now");
        }
    }

    private IEnumerator FillingGlassCroutine()
    {
        isFillingGlass = true;
        beerProducingIndicator.enabled = true;
        currentPlayerSpeed = player.PlayerSpeed;
        player.PlayerSpeed = 0;
        float time = GameConfigManager.FillingBeerGlassTime;
        float elapsedTime = 0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            beerProducingIndicator.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            yield return null;
        }
        player.PlayerSpeed = currentPlayerSpeed;
        isFillingGlass = false;
        beerProducingIndicator.enabled = false;
        player.TakeGlassOfBeer(currentBeerType);
    }
   
private void UpdadeBeerTypeSprite() => beerTypeImage.sprite = currentBeerType == 1 ? greenBeerGlassSprite : redBeerGlassSprite;
}
