using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Character")]
public class Character : ScriptableObject {
    [SerializeField] public GameObject prefab;
}
