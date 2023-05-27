using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/Resource")]
public class Resource : ScriptableObject {
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    public event UnityAction OnChange;

    private int _amount = 0;
    public int Amount {
        get => _amount;
        set {
            _amount = value;
            OnChange?.Invoke();
        }
    }
}
