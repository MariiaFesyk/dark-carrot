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
    [SerializeField] private WorldState state;
    [SerializeField] private PlayerInput inputSystem;
    [SerializeField] private float typingSpeed;
	
    private DialogueController dialogueController;
    public GameObjectOverride[] gameObjectOverrides;
    public DialogueGraph dialogue;

    [Header("UI")]
    [SerializeField] private TMP_Text lines;
    [SerializeField] private RectTransform optionsList;
    [SerializeField] private DialogueOptionDisplay optionPrefab;

    public UnityEvent OnDialogEnd = new UnityEvent();

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
        dialogueController.Events.End.AddListener(() => {
            gameObject.SetActive(false);
            state.globalTimeScale = 1f;
            inputSystem.actions.FindActionMap("Player").Enable();
            OnDialogEnd.Invoke();
        });
        dialogueController.Events.NodeEnter.AddListener((node) => {
        });
    }

    public void OpenDialogue(DialogueGraph dialogue){
        gameObject.SetActive(true);
        state.globalTimeScale = 0f;
		state.elapsed += 10;
        inputSystem.actions.FindActionMap("Player").Disable();
        
        this.dialogue = dialogue;

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


  