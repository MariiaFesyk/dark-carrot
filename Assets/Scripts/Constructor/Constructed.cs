using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructed : MonoBehaviour {
    [field: SerializeField] public Vector2Int size { get; private set; } = Vector2Int.one;
    private Vector2Int position;
}
