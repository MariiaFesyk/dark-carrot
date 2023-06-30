using UnityEngine;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues;

[CreateMenu("Custom/Sign Contract")]
public class SignContractAction : ActionDataBase {
    [SerializeField] public Contract contract;

    public override void OnInit(IDialogueController dialogue){ }

    public override void OnStart(){
    }

    public override ActionStatus OnUpdate(){
        return ActionStatus.Success;
    }

    public override void OnExit(){
    }

    public override void OnReset(){
    }
}
