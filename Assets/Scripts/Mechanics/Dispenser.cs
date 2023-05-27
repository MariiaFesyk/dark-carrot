using UnityEngine;

[RequireComponent(typeof(ItemHolder))]
public class Dispenser : Interactable {
    public override bool CanInteract(InteractionController interacting){
        return interacting.GetComponent<ItemHolder>()?.isEmpty() == true;
    }

    public override void OnInteraction(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        holder.InsertItem(GetComponent<ItemHolder>().RetrieveItem(false));
    }
}
