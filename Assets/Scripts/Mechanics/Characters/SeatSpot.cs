using UnityEngine;

public class SeatSpot : Interactable {
    [SerializeField] private string[] tags;
    public bool Contains(string tag) => System.Array.IndexOf<string>(tags, tag) != -1;

    private GameObject occupied = null;
    private Interactable reference = null;
    public bool IsEmpty => occupied == null;

    public void Set(GameObject gameObject){
        SetHighlight(false);
        occupied = gameObject;
        reference = occupied?.GetComponent<Interactable>();
        //TODO restore highlight
    }

    //TODO replace with something better
    public override void OnTriggerStay2D(Collider2D collider){
        reference?.OnTriggerStay2D(collider);
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