using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemHolder))]
public class CraftingDevice : Interactable {
    [System.Serializable]
    public class CraftingRecipe {
        public Item[] inputList;
        public Item input;
        public Item output;
        public float duration;

        public bool Validate(Item[] items){
            if(inputList.Length == 0) return items.Length == 1 && input == items[0];

            //TODO what the behaviour should be?
            if(items.Length != inputList.Length) return false;

            int mask = 0;
            foreach(var item in inputList){
                bool matched = false;
                for(int i = 0; i < items.Length; i++)
                    if(((1 << i) & mask) == 0 && items[i] == item){
                        mask |= 1 << i;
                        matched = true;
                        break;
                    }
                if(!matched) return false;
            }
            return true;
        }
    }
    [System.Serializable]
    public enum DeviceState {
        Idle,
        Processing,
        Finished,
    }

    [SerializeField] public List<CraftingRecipe> recipes = new();
    [SerializeField] private Item placeholder;
    [SerializeField] private Image progressIndicator;

    [SerializeField] private ItemHolder[] slots;

    private DeviceState state = DeviceState.Idle;
    private float elapsedTime = 0f;

    public override bool CanInteract(InteractionController interacting){
        switch(state){
            case DeviceState.Idle:
                //TODO refactor if this gets approved
                var item = interacting.GetComponent<ItemHolder>()?.Item;
                var empty = System.Array.Exists(slots, slot => slot.Quantity == 0);
                if(item != null && empty) return true;
                else if(item == null) return true;
                else return false;

                // if(GetComponent<ItemHolder>().Quantity > 0) return false;

                // var item = interacting.GetComponent<ItemHolder>()?.Item;
				// return item != null;
                // //return recipes.Exists(recipe => recipe.input == item);

            case DeviceState.Finished:
                return interacting.GetComponent<ItemHolder>()?.Quantity == 0;
                
            case DeviceState.Processing:
            default: return false;
        }
    }
    public override void OnInteraction(InteractionController interacting){
        if(state == DeviceState.Idle){
            var item = interacting.GetComponent<ItemHolder>().RetrieveItem();
            var empty = System.Array.FindIndex(slots, slot => slot.Quantity == 0);

            if(item != null && empty != -1){
                slots[empty].InsertItem(item);
            }else if(item == null){
                var items = System.Array.FindAll(System.Array.ConvertAll(slots, slot => slot.RetrieveItem()), item => item != null);
                for(int i = 1; i < slots.Length; i++) slots[i].gameObject.SetActive(false);
                var recipe = recipes.Find(recipe => recipe.Validate(items));
                if(recipe == null){
                    GetComponent<ItemHolder>().InsertItem(placeholder);
                    state = DeviceState.Finished;
                }else{
                    GetComponent<ItemHolder>().InsertItem(recipe.output);
                    StartCoroutine(CraftingCoroutine(recipe));
                }
            }




            // GetComponent<ItemHolder>().InsertItem(item);

            // var recipe = recipes.Find(recipe => recipe.input == item);
			// if(recipe == null){
			// 	GetComponent<ItemHolder>().InsertItem(placeholder);
			// 	state = DeviceState.Finished;
			// }else{
			// 	GetComponent<ItemHolder>().InsertItem(recipe.output);
			// 	StartCoroutine(CraftingCoroutine(recipe));
			// }
        }else if(state == DeviceState.Finished){
            interacting.GetComponent<ItemHolder>().InsertItem(GetComponent<ItemHolder>().RetrieveItem());
            state = DeviceState.Idle;

            for(int i = 1; i < slots.Length; i++) slots[i].gameObject.SetActive(true);
        }
    }

    IEnumerator CraftingCoroutine(CraftingRecipe recipe){
        state = DeviceState.Processing;
        progressIndicator.enabled = true;

        elapsedTime = 0f;
        while(elapsedTime < recipe.duration){
            elapsedTime += Phase.last.deltaTime;
            progressIndicator.fillAmount = Mathf.Min(1f, elapsedTime / recipe.duration);
            yield return null;
        }

        state = DeviceState.Finished;
        progressIndicator.enabled = false;
    }
}
