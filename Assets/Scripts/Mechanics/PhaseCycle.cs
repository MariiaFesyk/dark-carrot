using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCycle : MonoBehaviour {
    [SerializeField] private Phase[] phases;

    void Start(){
        Load();
    }

    public void Load(){
        phases[0]?.Enter();
    }

    void Update(){
        foreach(var phase in phases) phase.Update();
    }

    public void Pause(){
        Time.timeScale = 0.0f;
    }
    public void Resume(){
        Time.timeScale = 1.0f;
    }
}
