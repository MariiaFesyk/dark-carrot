using UnityEngine;

public class Jukebox : Interactable {
    [System.Serializable]
    public struct MusicTheme {
        public AudioSource audio;
    }

    [SerializeField] private MusicTheme[] themes;
    private int index = 0;

    public override bool CanInteract(InteractionController interacting){
        return interacting.GetComponent<ItemHolder>()?.Item == null;
    }
    public override void OnInteraction(InteractionController interacting){
        if(index < themes.Length) themes[index].audio.Stop();
        index = (index + 1) % (themes.Length + 1);
        if(index < themes.Length) themes[index].audio.Play();
    }
}
