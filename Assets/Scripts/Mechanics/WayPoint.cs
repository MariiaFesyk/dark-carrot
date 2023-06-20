using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
    [SerializeField] private string[] tags;
    //TODO fully delegate to UnitSpot, keep empty
    private GameObject occupied = null;
    public bool IsEmpty => occupied == null;
    
    public bool Contains(string tag) => System.Array.IndexOf<string>(tags, tag) != -1;
    public void Leave(){
        occupied = null;
        GetComponent<UnitSpot>()?.Set(null);
    }
    public void Reserve(GameObject gameObject){
        occupied = gameObject;
        GetComponent<UnitSpot>()?.Set(gameObject);
    }
}
