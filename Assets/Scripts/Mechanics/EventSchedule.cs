using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSchedule : MonoBehaviour {
    [SerializeField] private Phase phase;
    [SerializeField] private Contract contract;
    [SerializeField] private GameObject enableOnSecondDay;
    [SerializeField] private GameObject enableOnContract;

    void Start(){
        enableOnSecondDay.SetActive(false);
        enableOnContract.SetActive(false);
    }

    void OnEnable(){
        phase.OnPhaseEnter += OnPhaseTransition;
        if(contract) contract.OnChange += () => {
            enableOnContract.SetActive(true);
        };
    }

    void OnPhaseTransition(Phase phase){
        if(phase.enabled && phase.count == 2){
            enableOnSecondDay.SetActive(true);
            var queue = FindObjectOfType<VisitorQueue>();
            if(queue) queue.available = queue.QueryAvailable();
        }
    }
}
