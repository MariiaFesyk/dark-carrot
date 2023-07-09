using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ConstructionLayer : MonoBehaviour, AStar.IGrid2D {
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap walls;

    protected Tilemap tilemap { get; private set; }
    private Dictionary<Vector3Int, Constructed> grid = new();

    protected void Awake(){
        tilemap = GetComponent<Tilemap>();
    }

    void Start(){
        var predefined = GetComponentsInChildren<Constructed>();
        foreach(var constructed in predefined){
            Vector3Int cellPosition = tilemap.WorldToCell(constructed.transform.position);
            Vector2Int cellSize = constructed.size;
            Fill(cellPosition, cellSize, constructed);
        }
    }

    public void Place(Vector3 worldPosition, Constructible variant){
        var cellCoords = tilemap.WorldToCell(worldPosition);
    }

    private void Fill(Vector3Int origin, Vector2Int size, Constructed value){
        for(int x = 0; x < size.x; x++)
        for(int y = 0; y < size.y; y++){
            Vector3Int key = origin + new Vector3Int(x, y, 0);
            if(value != null) grid.Add(key, value);
            else grid.Remove(key);
        }
    }

    private static Vector2Int[] adjacent = new Vector2Int[]{
        Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right,
    };
    public IEnumerable<Vector2Int> GetAdjacent(Vector2Int cell){
        foreach(var offset in adjacent){
            Vector2Int next = offset + cell;
            if(grid.ContainsKey((Vector3Int) next)) continue;
            if(floor?.HasTile((Vector3Int) next) == false) continue;
            if(walls?.HasTile((Vector3Int) next) == true) continue;
            yield return next;
        }
    }
}
