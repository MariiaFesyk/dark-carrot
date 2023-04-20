using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningMaterials : PlayerInteractable
{
    public override void PlayerInteraction()
    {
        if (player.isHoldingCleaningMaterials)
        {
            player.ReleaseCleaningMaterials();
        }
        else
        {
            player.TakeCleaningMaterials();
        }
    }
}
