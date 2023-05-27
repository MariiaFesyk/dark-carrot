using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ConstructionLayer : MonoBehaviour
{

    protected Tilemap _tilemap { get; private set; }
    private Dictionary<Vector3Int, Constructed> _grid = new();

    protected void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void Place(Vector3 worldPosition, Constructible variant)
    {
        var cellCoords = _tilemap.WorldToCell(worldPosition);
    }
    
    [Serializable]
    public class Constructed {

    }
}
