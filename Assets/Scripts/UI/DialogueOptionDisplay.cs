using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueOptionDisplay : MonoBehaviour {
    public Text title;
    public Button button;
    public UnityEvent<int> clickEvent = new ActivateChoiceIndexEvent();

    private class ActivateChoiceIndexEvent : UnityEvent<int> {}

    private void Awake(){
        button.onClick.AddListener(() => {
            clickEvent.Invoke(transform.GetSiblingIndex());
        });
    }
}
