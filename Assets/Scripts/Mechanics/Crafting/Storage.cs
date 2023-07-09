using UnityEngine;

public class Storage : Interactable {
    [SerializeField] private Tag[] filter;

    private bool Filter(Item item){
        if(filter.Length == 0) return true;
        foreach(var tag in filter)
            if(System.Array.IndexOf<Tag>(item.Tags, tag) != -1) return true;
        return false;
    }

    public override bool CanInteract(InteractionController interacting){
        var storage = GetComponent<ItemHolder>();
        var holder = interacting.GetComponent<ItemHolder>();

        if(holder == null) return false;
        else if(storage == null) return holder.Quantity > 0;

        if(holder.Quantity > 0){
            if(!Filter(holder.Item)) return false;
            if(holder.Item != storage.Item && storage.Quantity > 1) return false;
            else if(holder.Item == storage.Item && storage.Quantity + holder.Quantity >= storage.Capacity) return false;
        }

        return holder.Quantity > 0 || storage.Quantity > 0;
    }

    public override void OnInteraction(InteractionController interacting){
        var storage = GetComponent<ItemHolder>();
        var holder = interacting.GetComponent<ItemHolder>();

        var item = holder.RetrieveItem();
        if(storage == null) return;

        if(storage.Quantity > 0 && storage.Item != item)
            holder.InsertItem(storage.RetrieveItem());
        
        if(item != null) storage.InsertItem(item);
    }
}
