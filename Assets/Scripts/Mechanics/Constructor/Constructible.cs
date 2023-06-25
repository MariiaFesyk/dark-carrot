using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Constructible", fileName = "New Constructible")]
public class Constructible : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    
    [field: SerializeField]
    public GameObject prefab { get; private set; }
}
