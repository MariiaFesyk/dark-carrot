using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable {
    public override bool CanInteract(InteractionController interacting){
        //TODO item filter?
        return interacting.GetComponent<ItemHolder>()?.isEmpty() == false;
    }

    public override void OnInteraction(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        var item = holder.RetrieveItem(true);
        var storage = GetComponent<ItemHolder>();
        if(storage != null){
            storage.InsertItem(item);
        }
    }
}
