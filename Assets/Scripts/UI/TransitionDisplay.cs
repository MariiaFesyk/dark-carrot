using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionDisplay : MonoBehaviour {
    [SerializeField] private WorldState state;
    [SerializeField] private UnityEvent OnWorkingPhaseStart;
    [SerializeField] private UnityEvent OnRestingPhaseStart;
    void OnEnable(){
        state.OnPhaseTransition += OnPhaseTransition;
    }
    void OnDisable(){
        state.OnPhaseTransition -= OnPhaseTransition;
    }
    void OnPhaseTransition(WorldState.WorldPhase phase){
        if(phase == WorldState.WorldPhase.Working) OnWorkingPhaseStart?.Invoke();
        else if(phase == WorldState.WorldPhase.Resting) OnRestingPhaseStart?.Invoke();
    }
}
