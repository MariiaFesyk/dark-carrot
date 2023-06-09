using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Tag")]
public class Tag : ScriptableObject {
    [field: SerializeField]
    public string Name { get; private set; }
	
	[field: SerializeField]
    public bool isVariant { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }
}
