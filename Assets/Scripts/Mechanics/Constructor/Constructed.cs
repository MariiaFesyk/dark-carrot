using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructed : MonoBehaviour {
    [field: SerializeField] public Vector2Int size { get; private set; } = Vector2Int.one;
    private Vector2Int position;
    private Grid grid;
    void Awake(){
        grid = GetComponentInParent<Grid>();
        position = (Vector2Int) grid.WorldToCell(transform.position);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected(){
        grid = GetComponentInParent<Grid>();
        if(grid == null) return;
        position = (Vector2Int) grid.WorldToCell(transform.position);

        var v0 = grid.CellToWorld(new Vector3Int(position.x, position.y, 0));
        var v1 = grid.CellToWorld(new Vector3Int(position.x + size.x, position.y, 0));
        var v2 = grid.CellToWorld(new Vector3Int(position.x + size.x, position.y + size.y, 0));
        var v3 = grid.CellToWorld(new Vector3Int(position.x, position.y + size.y, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(v0, v1);
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v0);
    }
#endif
}
