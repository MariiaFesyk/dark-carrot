using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/Contract", fileName = "New Contract")]
public class Contract : ScriptableObject {
    [SerializeField] public GameObject prefab;

    public event UnityAction OnChange;

    public ContractStatus status = ContractStatus.None;

    public void Sign(){
        status = ContractStatus.Signed;
        OnChange?.Invoke();
    }

    void OnEnable(){
        status = ContractStatus.None;
    }

    public enum ContractStatus {
        None = 0b00,
        Signed = 0b01,
        Fulfilled = 0b10,
    }    
}
