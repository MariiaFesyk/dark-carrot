using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour {
    public interface IInteractor {
        void OnInteractableEnter(InteractionTrigger trigger);
        void OnInteractableExit(InteractionTrigger trigger);
    }

    [SerializeField] private Interactable interactable;
    [SerializeField] private bool selectable = true;
    public int priority => interactable == null ? 0 : interactable.priority;
    public bool highlight {
        get => interactable?.highlight == true;
        set {
            if(interactable) interactable.highlight = value;
        }
    }
    public Interactable Interactable {
        get => interactable;
        set {
            if(interactable != value) highlight = false;
            interactable = value;
        }
    }

    private void OnDestroy(){
        highlight = false;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        interactable?.OnInteractableEnter(collider);
        IInteractor interacting = collider.GetComponent<InteractionController>();
        if(selectable) interacting?.OnInteractableEnter(this);
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        interactable?.OnInteractableExit(collider);
        IInteractor interacting = collider.GetComponent<InteractionController>();
        interacting?.OnInteractableExit(this);
    }
    //TODO perf, replace with something better
    private void OnTriggerStay2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        interactable?.OnTriggerStay2D(collider);
    }

    public bool CanInteract(InteractionController interacting) => interactable?.CanInteract(interacting) == true;
    public void OnInteraction(InteractionController interacting) => interactable?.OnInteraction(interacting);
}
