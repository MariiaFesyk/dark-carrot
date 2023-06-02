using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
    [SerializeField] private int layer;
    
    private void OnTriggerEnter2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        var interacting = collider.GetComponent<InteractionController>();
        interacting?.OnInteractableEnter(this);
    }
    private void OnTriggerExit2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        var interacting = collider.GetComponent<InteractionController>();
        interacting?.OnInteractableExit(this);
    }

    public void SetHighlight(bool highlight){
        if(highlight) transform.MoveLayer(0, layer);
        else transform.MoveLayer(layer, 0);
    }

    public abstract bool CanInteract(InteractionController interacting);
    public abstract void OnInteraction(InteractionController interacting);
}
