using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxCollector : Interactable {
    [SerializeField] private WorldState state;

    public override bool CanInteract(InteractionController interacting){
        return state.Phase == WorldState.WorldPhase.Resting;
    }
    public override void OnInteraction(InteractionController interacting){
        state.StartPhase(WorldState.WorldPhase.Working);
    }
}
