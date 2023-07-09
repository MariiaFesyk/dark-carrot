using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    [field: SerializeField] public int priority { get; private set; }

    //TODO move into "Highlight" component?
    [SerializeField] private ObjectSet layerSet;
    public virtual bool highlight {
        get => layerSet?.Contains(gameObject) == true;
        set {
            if(value) layerSet?.Add(gameObject);
            else layerSet?.Remove(gameObject);
        }
    }

    protected virtual void OnDestroy(){
        layerSet?.Remove(gameObject);
    }

    public virtual void OnInteractableEnter(Collider2D collider){}
    public virtual void OnInteractableExit(Collider2D collider){}
    public virtual void OnTriggerStay2D(Collider2D collider){}

    public abstract bool CanInteract(InteractionController interacting);
    public abstract void OnInteraction(InteractionController interacting);
}
