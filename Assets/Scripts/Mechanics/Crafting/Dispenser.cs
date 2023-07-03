using UnityEngine;

[RequireComponent(typeof(ItemHolder))]
public class Dispenser : Interactable {
    [SerializeField] private bool refund;

    public override bool CanInteract(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        var item = GetComponent<ItemHolder>().Item;

        if(holder?.Quantity == 0) return true;
        if(refund && holder?.Item == item) return true;
        return false;
    }

    public override void OnInteraction(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        if(holder.Quantity == 0) holder.InsertItem(GetComponent<ItemHolder>().Item);
        else holder.RetrieveItem();
    }
}
