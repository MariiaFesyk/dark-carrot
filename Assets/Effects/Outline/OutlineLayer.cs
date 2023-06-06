using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class OutlineLayer : MonoBehaviour {
    [SerializeField] private ObjectSet layerSet;
    [SerializeField] private Vector3 sortingAxis = Vector3.forward;
    [SerializeField] private int layer;
    private Canvas canvas;
    private float depth = 0f;

    void OnEnable(){
        canvas = GetComponent<Canvas>();

        if(layerSet){
            layerSet.OnAdd += OnAdd;
            layerSet.OnRemove += OnRemove;
            foreach(var gameObject in layerSet) OnAdd(gameObject);
        }
    }
    void OnDisable(){
        if(layerSet){
            layerSet.OnAdd -= OnAdd;
            layerSet.OnRemove -= OnRemove;
            foreach(var gameObject in layerSet) OnRemove(gameObject);
        }
    }
    void Update(){
        Vector3 cameraPosition = canvas.worldCamera.transform.position;
        float delta = depth - Vector3.Dot(sortingAxis, cameraPosition);
        canvas.planeDistance = delta / sortingAxis.z;
    }

    void OnAdd(GameObject gameObject){
        gameObject.transform.MoveLayer(0, layer);

        depth = Vector3.Dot(sortingAxis, gameObject.transform.position);
        canvas.enabled = layerSet.Count > 0;
    }
    void OnRemove(GameObject gameObject){
        gameObject.transform.MoveLayer(layer, 0);
        canvas.enabled = layerSet.Count > 0;
    }
}
