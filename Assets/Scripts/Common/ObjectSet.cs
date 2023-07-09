using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjectSet")]
public class ObjectSet : ScriptableObject, IEnumerable<GameObject> {
    public delegate void GameObjectEvent(GameObject gameObject);
    private List<GameObject> gameObjects = new List<GameObject>();

    public event GameObjectEvent OnAdd;
    public event GameObjectEvent OnRemove;

    public void Add(GameObject gameObject){
        gameObjects.Add(gameObject);
        OnAdd?.Invoke(gameObject);
    }
    public void Remove(GameObject gameObject){
        if(gameObjects.Remove(gameObject))
            OnRemove?.Invoke(gameObject);
    }
    public bool Contains(GameObject gameObject) => gameObjects.Contains(gameObject);

    public IEnumerator<GameObject> GetEnumerator() => gameObjects.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => gameObjects.Count;
}