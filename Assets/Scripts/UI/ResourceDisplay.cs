using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour {
    [SerializeField] private Resource resource;
    [SerializeField] private TMP_Text amountText;
    
    void OnEnable(){
        resource.OnChange += OnChange;
        OnChange();
    }
    void OnDisable(){
        resource.OnChange -= OnChange;
    }
    void OnChange(){
        amountText.text = resource.Amount.ToString();
    }
}
