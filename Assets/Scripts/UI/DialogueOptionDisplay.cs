using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueOptionDisplay : MonoBehaviour {
    public TMP_Text message;
    public Button button;
    public UnityEvent<int> clickEvent = new ActivateChoiceIndexEvent();

    private class ActivateChoiceIndexEvent : UnityEvent<int> {}
    [HideInInspector] public int index;

    private void Awake(){
        button.onClick.AddListener(() => {
            // clickEvent.Invoke(transform.GetSiblingIndex());
            SelectOption();
            clickEvent.Invoke(index);
        });
    }

    private void SelectOption(){
        foreach(Transform other in transform.parent)
            if(other != transform){
                var otherOption = other.GetComponent<DialogueOptionDisplay>();
                if(otherOption != null && otherOption.button.enabled) Destroy(other.gameObject);
            }
        button.enabled = false;
    }
}
