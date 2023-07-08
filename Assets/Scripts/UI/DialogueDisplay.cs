using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Actions;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueDisplay : MonoBehaviour {
    [SerializeField] private PlayerInput inputSystem;
    [SerializeField] private ContractDisplay contractController;
	
    private DialogueController dialogueController;
    public GameObjectOverride[] gameObjectOverrides;
    public DialogueGraph dialogue;

    [Header("UI")]
    [SerializeField] private float typingSpeed;
    [SerializeField] private RectTransform view;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private DialogueOptionDisplay optionPrefab;

    [Header("Character")]
    [SerializeField] private Image character;

    public UnityEvent OnDialogOpened = new UnityEvent();
    public UnityEvent OnDialogClosed = new UnityEvent();
    [HideInInspector] public UnityAction DialogCallback;

    private void Awake(){
        var database = new DatabaseInstanceExtended();
        dialogueController = new DialogueController(database);

        dialogueController.Events.Speak.AddListener((actor, text) => {
            if(actor == null) return;

            if(actor.Portrait != null) character.sprite = actor.Portrait; //TODO assign at start
            StartCoroutine(DisplayMessage(
                $"<size=120%>{actor.DisplayName}:</size> {text}", null
            ));
        });
        dialogueController.Events.Choice.AddListener((actor, text, choices) => {
            if(actor == null) return;

            if(actor.Portrait != null) character.sprite = actor.Portrait; //TODO assign at start
            StartCoroutine(DisplayMessage(
                $"<size=120%>{actor.DisplayName}:</size> {text}", choices.ConvertAll(choice => choice.Text).ToArray()
            ));
        });
        dialogueController.Events.End.AddListener(CloseDialogue);
        dialogueController.Events.NodeEnter.AddListener((node) => {
            //TODO better to refactor and add a custom contract node
            SignContractAction action = null;
            foreach(var exitAction in node.ExitActions){
                ActionRuntime actionRuntime = exitAction as ActionRuntime;
                if(actionRuntime == null) continue;
                SignContractAction actionData = actionRuntime.GetFieldValue<IActionData>("_data") as SignContractAction;
                if(actionData == null) continue;
                action = actionData;
                break;
            }
            if(action != null){
                gameObject.SetActive(false);
                contractController.callback = () => {
                    gameObject.SetActive(true);
                    dialogueController.Next();
                };
                contractController.OpenContract(action.contract);
            }
        });
    }

    private void CloseDialogue(){
        gameObject.SetActive(false);
        inputSystem.actions.FindActionMap("Player").Enable();

        OnDialogClosed.Invoke();
        DialogCallback?.Invoke();
        DialogCallback = null;

        foreach(Transform child in view) Destroy(child.gameObject);
    }

    public void OpenDialogue(DialogueGraph dialogue){
        gameObject.SetActive(true);
        inputSystem.actions.FindActionMap("Player").Disable();
        
        this.dialogue = dialogue;

        OnDialogOpened.Invoke();
        dialogueController.Play(dialogue, gameObjectOverrides.ToArray<IGameObjectOverride>());
    }

    private IEnumerator DisplayMessage(string message, string[] options){
        var box = Instantiate(messagePrefab, view);
        var lines = box.GetComponentInChildren<TMP_Text>();
        lines.text = message;
        LayoutRebuilder.ForceRebuildLayoutImmediate(view.parent as RectTransform);

        lines.maxVisibleCharacters = 0;
        lines.ForceMeshUpdate();
        var contentLength = lines.GetParsedText().Length;

        for(float elapsed = 0f; lines.maxVisibleCharacters < contentLength; elapsed += Time.unscaledDeltaTime){
            yield return null;
            lines.maxVisibleCharacters = 
            typingSpeed == 0f ? contentLength :
            Mathf.Min(contentLength, Mathf.FloorToInt(elapsed / typingSpeed));
        }

        if(options == null){
            //TODO use new input system
            while (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space)) {
                yield return null;
            }
            dialogueController.Next();
        }else{
            for(int i = 0; i < options.Length; i++){
                var option = Instantiate(optionPrefab, view);
                option.message.text = options[i];
                option.index = i;
                option.clickEvent.AddListener(dialogueController.SelectChoice);
            }
        }
    }

    private void Update(){
        dialogueController.Tick();
    }
}


  