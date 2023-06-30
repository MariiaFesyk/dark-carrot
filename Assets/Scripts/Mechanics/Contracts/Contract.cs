using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Contract", fileName = "New Contract")]
public class Contract : ScriptableObject {
    [SerializeField] public string header;
    [SerializeField, TextArea] public string conditions;
    [SerializeField, TextArea] public string obligations;

    public ContractStatus status = ContractStatus.None;

    void OnEnable(){
        status = ContractStatus.None;
    }

    public enum ContractStatus {
        None = 0b00,
        Signed = 0b01,
        Fulfilled = 0b10,
    }    
}
