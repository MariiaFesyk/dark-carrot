using UnityEngine;

public static class TransformExtension {
    public static void SetLayer(this Transform transform, int layer){
         transform.gameObject.layer = layer;
         foreach(Transform child in transform) child.SetLayer(layer);
    }
}
