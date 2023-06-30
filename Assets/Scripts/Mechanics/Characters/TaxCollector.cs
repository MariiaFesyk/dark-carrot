using UnityEngine;
using CleverCrow.Fluid.Dialogues.Graphs;

public class TaxCollector : Interactable {
    [SerializeField] private Phase phase;
    [SerializeField] private Resource collect;
    [SerializeField] private Character character;
    [SerializeField] private DialogueGraph dialogue;

    public override bool CanInteract(InteractionController interacting){
        return phase.enabled;
    }
    public override void OnInteraction(InteractionController interacting){
        var dialogueDisplay = FindObjectOfType<DialogueDisplay>(true);
        dialogueDisplay.DialogCallback += () => {

            phase.Exit();
        };
        dialogueDisplay.OpenDialogue(dialogue);
    }
}
