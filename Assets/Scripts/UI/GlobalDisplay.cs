using UnityEngine;
using UnityEngine.UI;

public class GlobalDisplay : MonoBehaviour {
    [SerializeField] private Image progressIndicator;
    [SerializeField] private Phase phase;
   
    void Start(){
        
    }

    void Update(){
        if(phase.enabled){
            progressIndicator.enabled = true;
            progressIndicator.fillAmount = phase.percent;
        }else{
            progressIndicator.enabled = false;
        }
    }
}
