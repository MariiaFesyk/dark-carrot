using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
    [SerializeField] private string[] tags;
    private GameObject occupied = null;
    public bool IsEmpty => occupied == null;
    
    public bool Contains(string tag) => System.Array.IndexOf<string>(tags, tag) != -1;
    public void Leave(){
        occupied = null;
    }
    public void Reserve(GameObject gameObject){
        occupied = gameObject;
    }
}
