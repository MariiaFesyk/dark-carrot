using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalDisplay : MonoBehaviour {
    [SerializeField] private Image progressIndicator;
    [SerializeField] private TMP_Text shiftIndicator;
    [SerializeField] private Phase phase;

    void OnEnable(){
        phase.OnPhaseEnter += OnPhaseTransition;
        phase.OnPhaseExit += OnPhaseTransition;
    }

    void Update(){
        if(phase.enabled){
            progressIndicator.enabled = true;
            progressIndicator.fillAmount = phase.percent;
        }else{
            progressIndicator.enabled = false;
        }
    }

    void OnPhaseTransition(Phase phase){
        var phaseType = this.phase.enabled ? "shift" : "rest";
        shiftIndicator.text = $"{phaseType} {this.phase.count}";
    }
}
