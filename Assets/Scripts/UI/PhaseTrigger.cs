using UnityEngine;
using UnityEngine.Events;

public class PhaseTrigger : MonoBehaviour {
    [SerializeField] private Phase phase;
    [SerializeField] public UnityEvent OnPhaseEnter;
    [SerializeField] public UnityEvent OnPhaseExit;

    void OnEnable(){
        phase.OnPhaseEnter += OnPhaseTransition;
        phase.OnPhaseExit += OnPhaseTransition;
    }
    void OnDisable(){
        phase.OnPhaseEnter -= OnPhaseTransition;
        phase.OnPhaseExit -= OnPhaseTransition;
    }

    private void OnPhaseTransition(Phase phase){
        if(phase.enabled) OnPhaseEnter?.Invoke();
        else OnPhaseExit?.Invoke();
    }
}
