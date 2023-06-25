using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueDisplay : MonoBehaviour {
    [SerializeField] private PlayerInput inputSystem;
    [SerializeField] private float typingSpeed;
	
    private DialogueController dialogueController;
    public GameObjectOverride[] gameObjectOverrides;
    public DialogueGraph dialogue;

    [Header("UI")]
    [SerializeField] private TMP_Text lines;
    [SerializeField] private RectTransform optionsList;
    [SerializeField] private DialogueOptionDisplay optionPrefab;

    public UnityEvent OnDialogOpened = new UnityEvent();
    public UnityEvent OnDialogClosed = new UnityEvent();
    [HideInInspector] public UnityAction DialogCallback;

    private void Awake(){
        var database = new DatabaseInstanceExtended();
        dialogueController = new DialogueController(database);

        dialogueController.Events.Speak.AddListener((actor, text) => {
            foreach(Transform child in optionsList) Destroy(child.gameObject);
            StartCoroutine(DisplayText(actor.DisplayName, text, true));
        });
        dialogueController.Events.Choice.AddListener((actor, text, choices) => {
            foreach(Transform child in optionsList) Destroy(child.gameObject);
            StartCoroutine(DisplayText(actor.DisplayName, text, false));
            foreach(var choice in choices){
                var option = Instantiate(optionPrefab, optionsList);
                option.title.text = choice.Text;
                option.clickEvent.AddListener(dialogueController.SelectChoice);
            }
        });
        dialogueController.Events.End.AddListener(CloseDialogue);
        dialogueController.Events.NodeEnter.AddListener((node) => {

        });
    }

    private void CloseDialogue(){
        gameObject.SetActive(false);
        inputSystem.actions.FindActionMap("Player").Enable();

        OnDialogClosed.Invoke();
        DialogCallback?.Invoke();
        DialogCallback = null;
    }

    public void OpenDialogue(DialogueGraph dialogue){
        gameObject.SetActive(true);
        inputSystem.actions.FindActionMap("Player").Disable();
        
        this.dialogue = dialogue;

        OnDialogOpened.Invoke();
        dialogueController.Play(dialogue, gameObjectOverrides.ToArray<IGameObjectOverride>());
    }

    private IEnumerator DisplayText(string actor, string text, bool autoprogress){
        lines.text = $"{actor}: ";
        foreach (char character in text.ToCharArray()){
            lines.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }
        if(autoprogress){
            while (!Input.GetMouseButtonDown(0)) {
                yield return null;
            }
            dialogueController.Next();
        }
    }

    private void Update(){
        dialogueController.Tick();
    }
}


  