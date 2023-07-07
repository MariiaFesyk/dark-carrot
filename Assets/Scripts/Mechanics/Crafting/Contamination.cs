using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contamination : Interactable {
    [SerializeField] private Item.ItemFilter filter;
    [SerializeField] private GameObject[] affected;
    [SerializeField] private SpriteRenderer display;
    [SerializeField] private GameObject mark;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool consume = true;

    private float percent = 0f;

    public override bool CanInteract(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();
        if(!holder) return false;

        return filter.Validate(holder.Item);
    }
    public override void OnInteraction(InteractionController interacting){
        var holder = interacting.GetComponent<ItemHolder>();

        var item = holder?.RetrieveItem();


        percent = 0f;
        display.sprite = null;
        display.gameObject.SetActive(false);
        mark.SetActive(false);
        foreach(var affect in affected) affect.SetActive(true);
    }

    public void Contribute(float amount){
        if(percent < 1f && percent + amount >= 1f){
            var sprite = sprites[Random.Range(0, sprites.Length)];
            display.sprite = sprite;
            display.gameObject.SetActive(true);
            mark.SetActive(true);

            foreach(var affect in affected) affect.SetActive(false);
        }
        percent += amount;
    }
}
