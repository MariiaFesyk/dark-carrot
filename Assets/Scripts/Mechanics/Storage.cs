using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable {
    public override bool CanInteract(InteractionController interacting){
		bool filled = GetComponent<ItemHolder>()?.isEmpty() == false;
        //TODO item filter?
        return interacting.GetComponent<ItemHolder>()?.isEmpty() == false || filled;
    }

    public override void OnInteraction(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        var item = holder.RetrieveItem(true);
        var storage = GetComponent<ItemHolder>();
        if(storage != null){
			var prevItem = storage.RetrieveItem(true);
			if(prevItem != null) holder.InsertItem(prevItem);
            storage.InsertItem(item);
        }
    }
}
