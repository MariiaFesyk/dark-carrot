using System.Collections.Generic;
using UnityEngine;

public class AStar {
    [System.Serializable]
    class CellNode2D {
        public Vector2Int position;
        public float cost, heuristic;
        public float total => cost + heuristic;
        public CellNode2D parent;
        public CellNode2D(Vector2Int position){
            this.position = position;
        }
    }
    public interface IGrid2D {
        IEnumerable<Vector2Int> GetAdjacent(Vector2Int cell);
    }
    private delegate float DistanceHeuristic(Vector2Int start, Vector2Int end);
    public static float Diagonal(Vector2Int start, Vector2Int end){
        return Mathf.Max(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
    }
    public static float Euclidean(Vector2Int start, Vector2Int end){
        return Vector2Int.Distance(start, end);
    }
    public static float Manhattan(Vector2Int start, Vector2Int end){
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    private DistanceHeuristic heuristic = Euclidean;
    private IGrid2D grid;
    private List<CellNode2D> queue = new();
    private Dictionary<Vector2Int, CellNode2D> visited = new();
    public AStar(IGrid2D grid){
        this.grid = grid;
    }
    public Vector2Int[] Search(Vector2Int origin, Vector2Int target){
        queue.Clear();
        visited.Clear();

        CellNode2D closest = new CellNode2D(origin);
        closest.heuristic = heuristic.Invoke(closest.position, target);
        queue.Add(closest);
        visited.Add(closest.position, closest);

        while(queue.Count > 0){
            int index = 0;
            float min = float.PositiveInfinity;
            for(int i = queue.Count - 1; i >= 0; i--)
                if(queue[i].total < min){
                    min = queue[i].total;
                    index = i;
                }
            var cell = queue[index];
            queue.SwapRemoveAt(index);

            var neighbours = grid.GetAdjacent(cell.position);
            foreach(var neighbour in neighbours){
                var g = cell.cost + heuristic(cell.position, neighbour);
                var h = heuristic(neighbour, target);

                CellNode2D adjacentCell;
                if(!visited.TryGetValue(neighbour, out adjacentCell)){
                    adjacentCell = new CellNode2D(neighbour);
                    visited.Add(neighbour, adjacentCell);
                }else if(adjacentCell.total < g + h) continue;

                adjacentCell.cost = g;
                adjacentCell.heuristic = h;
                adjacentCell.parent = cell;
                if(!queue.Contains(adjacentCell)) queue.Add(adjacentCell);

                if(adjacentCell.heuristic < closest.heuristic) closest = adjacentCell;
                if(closest.position == target) goto reconstruct;
            }
        }
        reconstruct:
        var path = new List<Vector2Int>{ closest.position };
        while(closest.parent != null){
            closest = closest.parent;
            path.Add(closest.position);
        }
        path.Reverse();
        return path.ToArray();
    }
}