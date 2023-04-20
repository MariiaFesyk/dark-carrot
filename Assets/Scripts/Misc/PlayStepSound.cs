using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepSound : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();    
    }

    public void StepSoundPlay()
    {
        audioManager.StepSoundPlay();
    }
}
