using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
    private GameObject occupied = null;
    public bool IsEmpty => occupied == null;
    
    public void Leave(){
        occupied = null;
    }
    public void Reserve(GameObject gameObject){
        occupied = gameObject;
    }
}
