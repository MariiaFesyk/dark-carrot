using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalDisplay : MonoBehaviour {
    [SerializeField] private Image progressIndicator;
    [SerializeField] private WorldState state;
   
    void Start(){
        
    }

    void Update(){
        if(state.Phase == WorldState.WorldPhase.Working){
            progressIndicator.enabled = true;
            progressIndicator.fillAmount = Mathf.Clamp01(state.Elapsed / state.WorkingPhaseDuration);
        }else{
            progressIndicator.enabled = false;
        }
    }
}
