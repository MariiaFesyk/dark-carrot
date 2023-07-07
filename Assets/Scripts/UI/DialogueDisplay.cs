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
    [SerializeField] private float typingSpeed;
    [SerializeField] private ContractDisplay contractController;
	
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
            if(actor == null) return;
            foreach(Transform child in optionsList) Destroy(child.gameObject);
            StartCoroutine(DisplayText(actor.DisplayName, text, true));
        });
        dialogueController.Events.Choice.AddListener((actor, text, choices) => {
            if(actor == null) return;
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
    }

    public void OpenDialogue(DialogueGraph dialogue){
        gameObject.SetActive(true);
        inputSystem.actions.FindActionMap("Player").Disable();
        
        this.dialogue = dialogue;

        OnDialogOpened.Invoke();
        dialogueController.Play(dialogue, gameObjectOverrides.ToArray<IGameObjectOverride>());
    }

    private IEnumerator DisplayText(string actor, string text, bool autoprogress){
        lines.text = $"<size=120%>{actor}:</size> {text}";
		lines.maxVisibleCharacters = 0;
		lines.ForceMeshUpdate();
		var content = lines.GetParsedText();

        for(float elapsed = 0f; lines.maxVisibleCharacters < content.Length; elapsed += Time.unscaledDeltaTime){
            yield return null;
            lines.maxVisibleCharacters = 
            typingSpeed == 0f ? content.Length :
            Mathf.Min(content.Length, Mathf.FloorToInt(elapsed / typingSpeed));
        }

        if(autoprogress){
            //TODO use new input system
            while (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space)) {
                yield return null;
            }
            dialogueController.Next();
        }
    }

    private void Update(){
        dialogueController.Tick();
    }
}


  