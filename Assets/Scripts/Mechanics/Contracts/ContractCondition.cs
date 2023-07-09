using UnityEngine;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;

[CreateMenu("Custom/Contract")]
public class ContractCondition : ConditionDataBase {
    [SerializeField] private Contract contract;
    [SerializeField] private Contract.ContractStatus status;

    public override void OnInit(IDialogueController dialogue){}
    public override bool OnGetIsValid(INode parent){
        return contract?.status == status;
    }
}