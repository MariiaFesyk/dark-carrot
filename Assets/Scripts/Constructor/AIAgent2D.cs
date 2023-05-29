using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent2D : MonoBehaviour {
    [SerializeField] public float movementSpeed;
    [SerializeField] public float stoppingDistance = 0f;

    public bool IsMoving => stepIndex != -1;

    private int stepIndex = -1;
    private Grid grid;
    private Vector2Int target;
    private AStar pathfinder = null;
    private Vector2Int[] path;
    void Awake(){
        grid = GetComponentInParent<Grid>();
        var layer = GetComponentInParent<ConstructionLayer>();
        if(layer != null) pathfinder = new AStar(layer);
    }
    void Update(){
        if(stepIndex < 0) return;
        var position = grid.CellToWorld((Vector3Int) path[stepIndex]);

        float distance = Vector2.Distance(transform.position, position);
        if(distance <= stoppingDistance){
            stepIndex++;
            if(stepIndex >= path.Length) stepIndex = -1;
        }else{
            float movement = Mathf.Clamp01((movementSpeed * Time.deltaTime) / distance);
            transform.position = Vector3.Lerp(transform.position, position, movement);
        }
    }
    public void SetDestination(Vector2 destination){
        target = (Vector2Int) grid.WorldToCell((Vector3) destination);
        var origin = (Vector2Int) grid.WorldToCell(transform.position);

        stepIndex = 0;
        if(pathfinder != null){
            path = pathfinder.Search(origin, target);
        }else{
            path = new Vector2Int[]{ origin, target };
        }
    }
}
