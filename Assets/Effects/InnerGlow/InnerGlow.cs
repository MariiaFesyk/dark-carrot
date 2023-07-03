using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class InnerGlow : MonoBehaviour {
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int sortingOrder;
    [Header("Shadow")]
    [SerializeField] private Color shadowColor = Color.black;
    [SerializeField, Range(0f, 1f)] private float intensity = 1f;

    void OnEnable(){
        GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        if(tilemap != null) GetComponent<MeshFilter>().mesh = GenerateMesh();
    }
    void OnDisable(){
        var meshFilter = GetComponent<MeshFilter>();
        if(meshFilter.mesh != null){
            GameObject.Destroy(meshFilter.mesh);
            meshFilter.mesh = null;
        }
    }

    private Mesh GenerateMesh(){
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);
        int columns = bounds.size.x + 2, rows = bounds.size.y + 2;
        bool[] fieldMask = new bool[columns * rows];

        for(int x = 0; x < bounds.size.x; x++)
        for(int y = 0; y < bounds.size.y; y++){
            TileBase tile = tiles[x + y * bounds.size.x];
            if(tile == null) continue;
            fieldMask[x + 1 + (y + 1) * columns] = true;
        }

        var fieldEDT = EuclideanDistanceTransform.compute(fieldMask, columns, rows, true);
        Dictionary<Vector2Int, int> mapping = new();

        List<Vector3> vertices = new();
        List<Color> colors = new();
        List<int> indices = new();

        Vector2Int[] corners = new Vector2Int[4]{
            Vector2Int.zero, Vector2Int.right, Vector2Int.up, Vector2Int.one
        };
        int[] quad_indices = new int[4];
        float[] quad_samples = new float[4];
        Color color = shadowColor.linear;

        for(int x = bounds.size.x * 2 - 1; x >= 0; x--)
        for(int y = bounds.size.y * 2 - 1; y >= 0; y--){
            Vector2 offset = new Vector2(0.5f * x, 0.5f * y);
            if(!fieldMask[(int) offset.x + 1 + ((int) offset.y + 1) * columns]) continue;

            float maxAlpha = 0f;
            for(int i = 0; i < corners.Length; i++){
                Vector2Int corner = corners[i] + new Vector2Int(x, y);
                var distance = fieldEDT.Sample(corner.x * 0.5f + 0.5f, corner.y * 0.5f + 0.5f);
                float weight = Mathf.Clamp01(distance * intensity);
                quad_samples[i] = 1f - weight;
                maxAlpha = Mathf.Max(maxAlpha, quad_samples[i]);
            }
            if(maxAlpha <= 0.0f) continue;

            for(int i = 0; i < corners.Length; i++){
                Vector2Int corner = corners[i] + new Vector2Int(x, y);
                if(mapping.TryGetValue(corner, out quad_indices[i])) continue;
                mapping.Add(corner, vertices.Count);
                quad_indices[i] = vertices.Count;

                vertices.Add(tilemap.CellToLocalInterpolated(bounds.position + new Vector3(
                    corner.x * 0.5f, corner.y * 0.5f, 0f
                )));

                colors.Add(new Color(color.r, color.g, color.b, color.a * quad_samples[i]));
            }
            
            indices.AddRange((x + y) % 2 == 0 ? new int[6]{
                quad_indices[0], quad_indices[2], quad_indices[3],
                quad_indices[3], quad_indices[0], quad_indices[1]
            } : new int[6]{
                quad_indices[0], quad_indices[2], quad_indices[1],
                quad_indices[1], quad_indices[2], quad_indices[3]
            });
        }
#if UNITY_EDITOR
        Debug.Log($"Tilemap shadow mesh: vertices {vertices.Count} indices {indices.Count}");
#endif
        return new Mesh {
            name = "Grid Mesh",
            vertices = vertices.ToArray(),
            triangles = indices.ToArray(),
            colors = colors.ToArray(),
        };
    }
}
