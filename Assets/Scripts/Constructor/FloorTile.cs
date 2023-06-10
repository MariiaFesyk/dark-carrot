using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Unity.Collections;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FloorTile : Tile {
    [Header("Tiling")]
    [SerializeField] public Vector2Int size = Vector2Int.one;
    [SerializeField] public Vector2 tileSize = Vector2.one;
    private Sprite[] sliced = new Sprite[0];

    void OnEnable(){
        sliced = SliceSprite(sprite, (Vector2Int) size);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap){
        base.RefreshTile(position, tilemap);
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = GetSprite(position);
    }

    private Sprite GetSprite(Vector3Int position){
        int x = position.x % size.x;
        int y = position.y % size.y;
        if(x < 0) x = x + size.x;
        if(y < 0) y = y + size.y;

        int index = x + y * size.x;
        return index < sliced.Length ? sliced[index] : null;
    }
    private Sprite[] SliceSprite(Sprite sprite, Vector2Int size){
        var indices = new NativeArray<ushort>(new ushort[6]{0,1,2,2,1,3}, Allocator.Temp);
        var vertices = new NativeArray<Vector3>(new Vector3[4]{
            tileSize * isometricToCartesian(new Vector2(-0.5f, 0.5f)),
            tileSize * isometricToCartesian(new Vector2(0.5f, 0.5f)),
            tileSize * isometricToCartesian(new Vector2(-0.5f, -0.5f)),
            tileSize * isometricToCartesian(new Vector2(0.5f, -0.5f)),
        }, Allocator.Temp);

        Sprite[] sliced = new Sprite[size.x * size.y];
        Vector2 invSize = Vector2.one / size;

        for(int x = 0; x < size.x; x++)
        for(int y = 0; y < size.y; y++){
            Sprite subsprite = Sprite.Create(sprite.texture, sprite.textureRect, Vector2.zero, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            
            Vector2[] uvs = new Vector2[4]{
                isometricToCartesian(invSize * new Vector2(x, y)),
                isometricToCartesian(invSize * new Vector2(x + 1, y)),
                isometricToCartesian(invSize * new Vector2(x, y + 1)),
                isometricToCartesian(invSize * new Vector2(x + 1, y + 1)),
            };

            for(int i = 0; i < uvs.Length; i++){
                float nx = Mathf.Clamp01(0.5f + uvs[i].y);
                float ny = Mathf.Clamp01(uvs[i].x);
                uvs[i].x = (subsprite.textureRect.x + nx * subsprite.textureRect.width) / subsprite.texture.width;
                uvs[i].y = (subsprite.textureRect.y + ny * subsprite.textureRect.height) / subsprite.texture.height;
            }
            var uvArray = new NativeArray<Vector2>(uvs, Allocator.Temp);

            subsprite.SetIndices(indices);
            subsprite.SetVertexAttribute(VertexAttribute.Position, vertices);
            subsprite.SetVertexAttribute(VertexAttribute.TexCoord0, uvArray);
            sliced[x + y * size.x] = subsprite;
        }
        return sliced;
    }
    private Vector2 isometricToCartesian(Vector2 isometric, float angle = Mathf.PI*0.5f){
        angle = Mathf.Sin(angle);
        return new Vector2(
            0.5f * (isometric.x + isometric.y / angle),
            0.5f * (isometric.x - isometric.y / angle)
        );
    }
    private Vector2 cartesianToIsometric(Vector2 cartesian, float angle = Mathf.PI*0.5f){
        angle = Mathf.Sin(angle);
        return new Vector2(
            cartesian.x + cartesian.y,
            (cartesian.x - cartesian.y) * angle
        );
    }

#if UNITY_EDITOR
    void OnValidate(){
        sliced = SliceSprite(sprite, (Vector2Int) size);
    }

    [MenuItem("Assets/Create/2D/Tiles/FloorTile")]
    public static void Create(){
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "asset", "Save Tile", "Assets");
        if(path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FloorTile>(), path);
    }
#endif
}
