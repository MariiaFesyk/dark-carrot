using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerIngredientBarrel :PlayerInteractable
{
    [SerializeField] private int beerIngredientType;
    [SerializeField] private GameObject greenBeerIngredientModel;
    [SerializeField] private GameObject redBeerIngredientModel;



    private void Start()
   {
        if(beerIngredientType == 1)
        {
            greenBeerIngredientModel.SetActive(true);
            redBeerIngredientModel.SetActive(false);
        }
        else
        {
            greenBeerIngredientModel.SetActive(false);
            redBeerIngredientModel.SetActive(true);
        }
    }

    public override void PlayerInteraction()
    {
        if(!player.isHoldingBeerKeg && !player.isHoldingCleaningMaterials && !player.isHoldingGlassOfBeer)
        {
            player.TakeBeerIngredient(beerIngredientType);
        }
        
    }

   
}
