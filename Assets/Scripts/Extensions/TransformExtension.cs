using UnityEngine;
using System.Reflection;

public static class TransformExtension {
    public static void MoveLayer(this Transform transform, int originLayer, int targetLayer){
        if(transform.gameObject.layer == originLayer)
            transform.gameObject.layer = targetLayer;
        foreach(Transform child in transform) child.MoveLayer(originLayer, targetLayer);
    }
}
//TODO remove
public static class ReflectionExtensions {
    public static T GetFieldValue<T>(this object obj, string name) {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var field = obj.GetType().GetField(name, bindingFlags);
        return (T)field?.GetValue(obj);
    }
}