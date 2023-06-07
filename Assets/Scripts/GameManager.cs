using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(gameObject);
        }else{
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public abstract class AbstractWorldState : ScriptableObject {
        public abstract IEnumerator Play();
    }

    [field: SerializeField] public AbstractWorldState world;
    private void Start(){
        StartCoroutine(world.Play());
    }
}
