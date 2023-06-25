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

    private void Start(){
    }
}
