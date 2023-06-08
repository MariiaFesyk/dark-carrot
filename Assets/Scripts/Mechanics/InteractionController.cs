using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour, InputActions.IPlayerActions {
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable selected = null;
    void Awake(){

    }
    void Update(){
        ReselectInteractable();
    }

    private void ReselectInteractable(){
        Interactable closest = null;
        float minDistanceSquared = float.MaxValue;
        foreach(Interactable interactable in interactables){
            float distanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;
            if(distanceSquared >= minDistanceSquared) continue;

            if(!interactable.CanInteract(this)) continue;
            minDistanceSquared = distanceSquared;
            closest = interactable;
        }
        if(closest != selected){
            selected?.SetHighlight(false);
            closest?.SetHighlight(true);
            selected = closest;
        }
    }

    public void OnInteractableEnter(Interactable interactable){
        interactables.Add(interactable);
    }
    public void OnInteractableExit(Interactable interactable){
        interactables.Remove(interactable);
    }

    public void OnMove(InputAction.CallbackContext context){}
    public void OnInteract(InputAction.CallbackContext context){
        if(!selected || !context.performed) return;
        selected.OnInteraction(this);
        ReselectInteractable();
    }

    public void OnTargetMove(InputAction.CallbackContext context){}
    public void OnTargetTrigger(InputAction.CallbackContext context){}
}
