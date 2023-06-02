using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {
    [SerializeField] private Item item = null;
    [SerializeField] private uint quantity;
    [SerializeField] private GameObject iconDisplay = null;
	[SerializeField] private GameObject tagIconDisplay = null;
    [SerializeField] private Animator animator = null;
	
	private void SetImage(GameObject target, Sprite icon){
		var spriteRenderer = target?.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null) spriteRenderer.sprite = icon;

        var imageRenderer = target?.GetComponent<Image>();
        if(imageRenderer != null){
            imageRenderer.sprite = icon;
            imageRenderer.enabled = icon != null;
        }
	}

    private void UpdateDisplay(){
        SetImage(iconDisplay, item?.Icon);
		
		var variantTag = item?.VariantTag;
		
		if(tagIconDisplay != null) SetImage(tagIconDisplay, variantTag?.Icon);

        if(animator != null) animator.SetBool("Carrying", !isEmpty());
    }
    public bool isEmpty(){
        return item == null;
    }
    public void InsertItem(Item nextItem){
        item = nextItem;
        UpdateDisplay();
    }
    public Item RetrieveItem(bool remove){
        Item prevItem = item;
        if(remove){
            item = null;
            UpdateDisplay();
        }
        return prevItem;
    }
#if UNITY_EDITOR
    void OnValidate(){
        //if(this.didStart) https://docs.unity3d.com/2023.1/Documentation/ScriptReference/MonoBehaviour-didStart.html
        UpdateDisplay();
    }
#endif
}
