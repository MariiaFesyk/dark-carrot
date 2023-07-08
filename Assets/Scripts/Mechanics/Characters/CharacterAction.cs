using UnityEngine;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues;

[CreateMenu("Custom/Character")]
public class CharacterAction : ActionDataBase {
    [SerializeField] public Character character;
	[SerializeField] public bool active;

    public override void OnInit(IDialogueController dialogue){ }

    public override void OnStart(){
    }

    public override ActionStatus OnUpdate(){
		character.active = active;
        return ActionStatus.Success;
    }

    public override void OnExit(){
    }

    public override void OnReset(){
    }
}