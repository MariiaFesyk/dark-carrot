using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ContractDisplay : MonoBehaviour {
    private Contract currentContract;
    [HideInInspector] public UnityAction callback;

    public void Sign(){
        Hide();

        currentContract.Sign();
        currentContract = null;

        callback?.Invoke();
        callback = null;
    }
    public void OpenContract(Contract contract){
        gameObject.SetActive(true);

        currentContract = contract;

        var view = Instantiate(contract.prefab, transform);
        var button = view.GetComponentInChildren<Button>();
        button.onClick.AddListener(Sign);
    }
    public void Hide(){
        foreach(Transform child in transform) Destroy(child.gameObject);
        gameObject.SetActive(false);
    }

}
