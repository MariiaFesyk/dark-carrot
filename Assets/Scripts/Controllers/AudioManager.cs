using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource coinsSound;
    [SerializeField] private AudioSource brewerySound;
    [SerializeField] private AudioSource visitorEntersSound;
    [SerializeField] private AudioSource stepSound;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CoinsAddedSoundPlay()
    {
        coinsSound.Play();
    }

    public void BrewerySoundPlay()
    {
        brewerySound.Play();
    }

    public void VisitorEntersSoundPlay()
    {
        visitorEntersSound.Play();
    }

    public void StepSoundPlay()
    {
        stepSound.Play();
    }
}
