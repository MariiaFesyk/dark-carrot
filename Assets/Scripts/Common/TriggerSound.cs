using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
public class TriggerSound : MonoBehaviour {
    private AudioSource audioSource;
    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundDefault(){
        audioSource.Play();
    }
    public void PlaySound(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
}
