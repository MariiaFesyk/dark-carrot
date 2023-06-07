using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemHolder))]
public class CraftingDevice : Interactable {
    [System.Serializable]
    public class CraftingRecipe {
        public Item input;
        public Item output;
        public float duration;
    }
    [System.Serializable]
    public enum DeviceState {
        Idle,
        Processing,
        Finished,
    }

    [SerializeField] private List<CraftingRecipe> recipes = new();
    [SerializeField] private Item placeholder;
    [SerializeField] private Image progressIndicator;
    private DeviceState state = DeviceState.Idle;
    private float elapsedTime = 0f;

    public override bool CanInteract(InteractionController interacting){
        switch(state){
            case DeviceState.Idle: 
                if(GetComponent<ItemHolder>().Quantity > 0) return false;

                var item = interacting.GetComponent<ItemHolder>()?.Item;
				return item != null;
                //return recipes.Exists(recipe => recipe.input == item);

            case DeviceState.Finished:
                return interacting.GetComponent<ItemHolder>()?.Quantity == 0;
                
            case DeviceState.Processing:
            default: return false;
        }
    }
    public override void OnInteraction(InteractionController interacting){
        if(state == DeviceState.Idle){
            var item = interacting.GetComponent<ItemHolder>().RetrieveItem();
            GetComponent<ItemHolder>().InsertItem(item);

            var recipe = recipes.Find(recipe => recipe.input == item);
			if(recipe == null){
				GetComponent<ItemHolder>().InsertItem(placeholder);
				state = DeviceState.Finished;
			}else{
				GetComponent<ItemHolder>().InsertItem(recipe.output);
				StartCoroutine(CraftingCoroutine(recipe));	
			}
        }else if(state == DeviceState.Finished){
            interacting.GetComponent<ItemHolder>().InsertItem(GetComponent<ItemHolder>().RetrieveItem());
            state = DeviceState.Idle;
        }
    }

    IEnumerator CraftingCoroutine(CraftingRecipe recipe){
        state = DeviceState.Processing;
        progressIndicator.enabled = true;

        elapsedTime = 0f;
        while(elapsedTime < recipe.duration){
            elapsedTime += Time.deltaTime;
            progressIndicator.fillAmount = Mathf.Min(1f, elapsedTime / recipe.duration);
            yield return null;
        }

        state = DeviceState.Finished;
        progressIndicator.enabled = false;
    }
}
