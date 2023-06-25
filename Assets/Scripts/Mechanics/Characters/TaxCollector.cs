using UnityEngine;

public class TaxCollector : Interactable {
    [SerializeField] private Phase phase;

    public override bool CanInteract(InteractionController interacting){
        return phase.enabled;
    }
    public override void OnInteraction(InteractionController interacting){
        phase.Exit();
    }
}
