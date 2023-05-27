using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour {
    [SerializeField] private Item item = null;
    [SerializeField] private GameObject iconDisplay = null;
    [SerializeField] private Animator animator = null;

    private void UpdateDisplay(){
        var spriteRenderer = iconDisplay?.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null) spriteRenderer.sprite = item?.Icon;

        var imageRenderer = iconDisplay?.GetComponent<Image>();
        if(imageRenderer != null){
            imageRenderer.sprite = item?.Icon;
            imageRenderer.enabled = !isEmpty();
        }

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
