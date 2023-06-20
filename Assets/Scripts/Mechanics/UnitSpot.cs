using UnityEngine;

public class UnitSpot : Interactable {
    private GameObject occupied = null;
    private Interactable reference = null;
    public bool IsEmpty => occupied == null;

    public void Set(GameObject gameObject){
        occupied = gameObject;
        reference = occupied?.GetComponent<Interactable>();
    }

    public override void SetHighlight(bool highlight){
        reference?.SetHighlight(highlight);
    }
    protected override void OnDestroy(){
        reference?.SetHighlight(false);
    }

    public override bool CanInteract(InteractionController interacting){
        return reference?.CanInteract(interacting) == true;
    }

    public override void OnInteraction(InteractionController interacting){
        reference?.OnInteraction(interacting);
    }
}