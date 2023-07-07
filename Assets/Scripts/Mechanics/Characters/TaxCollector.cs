using UnityEngine;
using CleverCrow.Fluid.Dialogues.Graphs;

public class TaxCollector : Interactable {
    [SerializeField] private Phase phase;
    [SerializeField] private Resource collect;
    [SerializeField] private Character character;
    [SerializeField] private DialogueGraph dialogue;
	[SerializeField] private GameObject dialogIcon;
	
	void OnEnable(){
		phase.OnPhaseEnter += OnPhaseTransition;
        phase.OnPhaseExit += OnPhaseTransition;
	}
	
	void OnPhaseTransition(Phase phase){
        if(phase.enabled){
			dialogIcon.SetActive(true);
		}else{
			dialogIcon.SetActive(false);
		}
    }
	
    public override bool CanInteract(InteractionController interacting){
        return phase.enabled;
    }
    public override void OnInteraction(InteractionController interacting){
        var dialogueDisplay = FindObjectOfType<DialogueDisplay>(true);
        dialogueDisplay.DialogCallback += () => {
            collect.Amount = 0;
            phase.Exit();
        };
        dialogueDisplay.OpenDialogue(dialogue);
    }
}
