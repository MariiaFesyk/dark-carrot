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
    [SerializeField] private Image progressIndicator;
    private DeviceState state = DeviceState.Idle;
    private float elapsedTime = 0f;

    public override bool CanInteract(InteractionController interacting){
        switch(state){
            case DeviceState.Idle: 
                if(!GetComponent<ItemHolder>().isEmpty()) return false;

                var item = interacting.GetComponent<ItemHolder>()?.RetrieveItem(false);
                return recipes.Exists(recipe => recipe.input == item);

            case DeviceState.Finished:
                return interacting.GetComponent<ItemHolder>()?.isEmpty() == true;
                
            case DeviceState.Processing:
            default: return false;
        }
    }
    public override void OnInteraction(InteractionController interacting){
        if(state == DeviceState.Idle){
            var item = interacting.GetComponent<ItemHolder>().RetrieveItem(true);
            GetComponent<ItemHolder>().InsertItem(item);

            var recipe = recipes.Find(recipe => recipe.input == item);
            GetComponent<ItemHolder>().InsertItem(recipe.output);
            StartCoroutine(CraftingCoroutine(recipe));
        }else if(state == DeviceState.Finished){
            interacting.GetComponent<ItemHolder>().InsertItem(GetComponent<ItemHolder>().RetrieveItem(true));
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
