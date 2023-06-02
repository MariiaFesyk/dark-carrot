using UnityEngine;

public static class TransformExtension {
    public static void MoveLayer(this Transform transform, int originLayer, int targetLayer){
        if(transform.gameObject.layer == originLayer)
            transform.gameObject.layer = targetLayer;
        foreach(Transform child in transform) child.MoveLayer(originLayer, targetLayer);
    }
}
