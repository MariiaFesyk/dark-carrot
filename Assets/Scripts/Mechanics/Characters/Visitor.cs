using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues;

[RequireComponent(typeof(AIAgent2D))]
public class Visitor : Interactable {
    [SerializeField] private ParticleSystem bubbleParticles;
	[SerializeField] private GameObject dialogIcon;
    [SerializeField] private Image progressIndicator;
    [SerializeField] private Image orderIcon;
    [SerializeField] private Sprite unknownOrderIcon;

    [System.Serializable]
    public class Schedule {
        public Phase phase;
        [SerializeField, Min(1)] public int start = 1;
        public bool[] mask;

        public bool Validate(){
            if(phase == null) return true;
            if(phase.count < start) return false;
            if(mask.Length == 0) return true;
            int index = (phase.count - start) % mask.Length;
            return mask[index];
        }
    }

    [SerializeField] public Schedule schedule;

    [SerializeField] private VisitorOrder[] orders;
    [SerializeField] public DialogueGraph dialogue;

    [System.Serializable]
    public enum VisitorState { //TODO use state machine library?
        Idle, Arriving, Leaving, AwaitingOrder, Consuming, Storytelling,
    }
    private AIAgent2D agent;
    private VisitorQueue queue;
    private VisitorOrder order;
    private VisitorState state = VisitorState.Idle;
    private SeatSpot target = null;
    private float elapsedTime = 0f;
    private int fulfilled = 0;

    void Awake(){
        agent = GetComponent<AIAgent2D>();
        queue = FindObjectOfType<VisitorQueue>();
    }

    public void Enter(SeatSpot waypoint){
        state = VisitorState.Arriving;
        target = waypoint;
        target.Set(gameObject);
        agent.SetDestination(waypoint.transform.position);
        agent.OnFinished += GenerateOrder;
    }

    public void OnWorkingPhaseEnd(){
        if(state == VisitorState.Idle){
            state = VisitorState.Storytelling;
			dialogIcon.SetActive(true);
        }else{
            Leave();
        }
    }

    public void Leave(){
        if(target == null) return;
        SetCoroutine(null);

        order = null;

        state = VisitorState.Leaving;
        target.Set(null);
		dialogIcon?.SetActive(false);

        //TODO move to event trigger?
        var dirt = target.GetComponentInParent<Contamination>();
        if(dirt != null && fulfilled > 0){
            dirt.Contribute(Random.Range(0.2f, 0.6f));
        }

        target = null;
        agent.SetDestination(queue.doorLocation.position);
        agent.OnFinished += () => {
            Destroy(gameObject);
        };
    }

    private void GenerateOrder(){
        agent.OnFinished -= GenerateOrder;
        order = orders[Random.Range(0, orders.Length)];
        SetCoroutine(AwaitingOrderCoroutine(order));
    }

    IEnumerator AwaitingOrderCoroutine(VisitorOrder order){
        try{
            orderIcon.transform.parent.gameObject.SetActive(true);
            // orderIcon.sprite = order.Icon;
            orderIcon.sprite = unknownOrderIcon;

            state = VisitorState.AwaitingOrder;
            progressIndicator.enabled = true;

            elapsedTime = 0f;
            while(elapsedTime < order.Duration && order != null){
                elapsedTime += queue.phase.deltaTime;
                progressIndicator.fillAmount = Mathf.Min(1f, elapsedTime / order.Duration);
                yield return null;
            }
            Leave();
        }finally{
            orderIcon.transform.parent.gameObject.SetActive(false);
            progressIndicator.enabled = false;
        }
    }
    IEnumerator ConsumingCoroutine(){
        try{
            state = VisitorState.Consuming;
            bubbleParticles.Play();
            yield return new WaitForSeconds(5f);

            if(dialogue != null){
                if(fulfilled < 3){
                    GenerateOrder();
                }else{
                    state = VisitorState.Idle;
                }
            }else{
                Leave();
            }
        }finally{
            bubbleParticles.Stop();
        }
    }

    //TODO refactor, custom radius for autotriggering effects?
    public override void OnTriggerStay2D(Collider2D collider){
        if(!collider.CompareTag("Player")) return;
        if(state == VisitorState.AwaitingOrder){
            orderIcon.sprite = order.Icon;
        }
    }

    public override bool CanInteract(InteractionController interacting){
        if(state == VisitorState.AwaitingOrder && order != null){
            var item = interacting.GetComponent<ItemHolder>()?.Item;
            return order.Validate(item);
        }else if(state == VisitorState.Storytelling && dialogue != null){
            return true;
        }else return false;        
    }

    public override void OnInteraction(InteractionController interacting){
        if(state == VisitorState.AwaitingOrder){
            var item = interacting.GetComponent<ItemHolder>().RetrieveItem();

            order?.Fulfill(item, this);
            order = null;
            fulfilled++;

            SetCoroutine(ConsumingCoroutine());
        }else if(state == VisitorState.Storytelling){
            SetCoroutine(null);
            var dialogueDisplay = FindObjectOfType<DialogueDisplay>(true);
            dialogueDisplay.DialogCallback += Leave;
            dialogueDisplay.OpenDialogue(dialogue);
        }
    }

    private IEnumerator coroutine;
    private void SetCoroutine(IEnumerator next){
        if(coroutine != null){
            (coroutine as System.IDisposable)?.Dispose();
            StopAllCoroutines();
        }
        coroutine = next;
        if(coroutine != null) StartCoroutine(coroutine);
    }
}
