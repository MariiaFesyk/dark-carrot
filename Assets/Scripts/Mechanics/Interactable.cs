using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
    [SerializeField] private ObjectSet layerSet;
    
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
    public virtual void OnTriggerStay2D(Collider2D collider){}

    public virtual void SetHighlight(bool highlight){
        if(highlight) layerSet?.Add(gameObject);
        else layerSet?.Remove(gameObject);
    }
    protected virtual void OnDestroy(){
        layerSet?.Remove(gameObject);
    }

    public abstract bool CanInteract(InteractionController interacting);
    public abstract void OnInteraction(InteractionController interacting);
}
