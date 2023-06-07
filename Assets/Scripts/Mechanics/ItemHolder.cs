using UnityEngine;

public class ItemHolder : MonoBehaviour {
    [SerializeField] private int capacity;
    [SerializeField] private ItemDisplay display;
    [SerializeField] private Animator animator = null;

    [SerializeField] private Item item = null;
    [SerializeField] private uint quantity = 0;

    public Item Item => item;
    public uint Quantity => quantity;
    public int Capacity => capacity;
	
    private void UpdateDisplay(){
        display.UpdateDisplay(item, quantity);
        if(animator != null) animator.SetBool("Carrying", Quantity > 0);
    }
    public void InsertItem(Item nextItem){
        if(nextItem != null && (item == nextItem || item == null)) quantity++;
        item = nextItem;
        UpdateDisplay();
    }
    public Item RetrieveItem(){
        Item prevItem = item;
        if(item != null) quantity--;
        if(quantity <= 0) item = null;
        UpdateDisplay();
        return prevItem;
    }
#if UNITY_EDITOR
    void OnValidate(){
        //if(this.didStart) https://docs.unity3d.com/2023.1/Documentation/ScriptReference/MonoBehaviour-didStart.html
        UpdateDisplay();
    }
#endif
}
