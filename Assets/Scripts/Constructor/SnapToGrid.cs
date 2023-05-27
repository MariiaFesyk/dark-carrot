using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode, RequireComponent(typeof(Grid))]
public class SnapToGrid : MonoBehaviour
{
    private Grid _grid;
    private void Awake(){
        _grid = GetComponent<Grid>();
    }

    #if UNITY_EDITOR
    private void OnEnable(){
        SceneView.duringSceneGui += OnSceneGui;
    }
    private void OnDisable(){
        SceneView.duringSceneGui -= OnSceneGui;
    }
    void OnSceneGui(SceneView view) {
        if(Selection.activeTransform && Selection.activeTransform.IsChildOf(transform) && Event.current.type == EventType.MouseUp){
            var cell = _grid.LocalToCell(Selection.activeTransform.position);
            Selection.activeTransform.position = _grid.CellToLocal(cell);
        }
    }
    #endif
}
