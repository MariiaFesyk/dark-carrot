using UnityEngine;

public class SeatSpot : MonoBehaviour {
    [SerializeField] private InteractionTrigger trigger;
    [SerializeField] private string[] tags;
    public bool Contains(string tag) => System.Array.IndexOf<string>(tags, tag) != -1;

    private GameObject occupied = null;
    public bool IsEmpty => occupied == null;

    public void Set(GameObject gameObject){
        occupied = gameObject;
        if(trigger) trigger.Interactable = occupied?.GetComponent<Interactable>();
    }
    private void OnDestroy(){
        if(trigger) trigger.Interactable = null;
    }
}