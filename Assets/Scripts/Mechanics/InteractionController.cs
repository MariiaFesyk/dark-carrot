using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour, InputActions.IPlayerActions, InteractionTrigger.IInteractor {
    private List<InteractionTrigger> interactables = new List<InteractionTrigger>();
    private InteractionTrigger selected = null;

    void Update(){
        ReselectInteractable();
    }

    private void ReselectInteractable(){
        InteractionTrigger closest = null;
        float minDistanceSquared = float.MaxValue;
        int maxPriority = int.MinValue;

        for(int i = interactables.Count - 1; i >= 0; i--){
            InteractionTrigger interactable = interactables[i];

            if(interactable.priority < maxPriority) continue;

            float distanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;
            if(distanceSquared >= minDistanceSquared) continue;

            if(!interactable.CanInteract(this)) continue;
            minDistanceSquared = distanceSquared;
            maxPriority = interactable.priority;
            closest = interactable;
        }
        if(closest != selected || closest?.highlight == false){
            if(selected) selected.highlight = false;
            if(closest) closest.highlight = true;
            selected = closest;
        }
    }

    public void OnInteractableEnter(InteractionTrigger interactable) => interactables.Add(interactable);
    public void OnInteractableExit(InteractionTrigger interactable) => interactables.Remove(interactable);

    public void OnMove(InputAction.CallbackContext context){}
    public void OnInteract(InputAction.CallbackContext context){
        if(!selected || !context.performed) return;
        selected.OnInteraction(this);
        ReselectInteractable();
    }

    public void OnTargetMove(InputAction.CallbackContext context){}
    public void OnTargetTrigger(InputAction.CallbackContext context){}
}
