using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour {
    [SerializeField] private GameObject iconDisplay = null;
    [SerializeField] private GameObject tagIconDisplay = null;
    [SerializeField] private TMP_Text quantityText = null;

    private void SetImage(GameObject target, Sprite icon){
        if(target == null) return;

		var spriteRenderer = target.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null) spriteRenderer.sprite = icon;

        var imageRenderer = target.GetComponent<Image>();
        if(imageRenderer != null){
            imageRenderer.sprite = icon;
            imageRenderer.enabled = icon != null;
        }
	}

    public void UpdateDisplay(Item item, uint quantity){
        SetImage(iconDisplay, item?.Icon);		
		SetImage(tagIconDisplay, item?.VariantTag?.Icon);
        if(quantityText != null){
            quantityText.enabled = quantity > 1;
            quantityText.text = quantity.ToString();
        }
    }
}
