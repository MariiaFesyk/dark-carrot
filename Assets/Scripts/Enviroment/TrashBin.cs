using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : PlayerInteractable
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if(player.isHoldingBeerIngredient || player.isHoldingBeerKeg || player.isHoldingGlassOfBeer)
            {
                player.SetTarget(gameObject);
                outline.OutlineWidth = 2.5f;
            }
        }
    }

    public override void PlayerInteraction()
    {
        player.ReleaseAnyItem();
    }

   
}
