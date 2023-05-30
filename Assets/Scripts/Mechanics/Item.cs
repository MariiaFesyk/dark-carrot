using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Item", fileName = "New Item")]
public class Item : ScriptableObject {
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public Tag[] Tags { get; private set; }
}
