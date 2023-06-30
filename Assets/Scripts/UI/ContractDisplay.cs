using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ContractDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text conditions;
    [SerializeField] private TMP_Text obligations;
    private Contract currentContract;
    [HideInInspector] public UnityAction callback;

    public void Sign(){
        Hide();

        currentContract.status = Contract.ContractStatus.Signed;
        currentContract = null;

        callback?.Invoke();
        callback = null;
    }
    public void OpenContract(Contract contract){
        gameObject.SetActive(true);

        currentContract = contract;

        header.text = contract.header;
        conditions.text = contract.conditions;
        obligations.text = contract.obligations;
    }
    public void Hide(){
        gameObject.SetActive(false);
    }

}
