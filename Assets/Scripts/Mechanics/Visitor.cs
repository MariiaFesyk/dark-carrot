using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AIAgent2D))]
public class Visitor : Interactable {
    [SerializeField] private ParticleSystem bubbleParticles;
    [SerializeField] private Image progressIndicator;

    [System.Serializable]
    public class Order {
        public float duration;
    }
    [System.Serializable]
    public enum VisitorState {
        Idle, Arriving, Leaving, AwaitingOrder, Consuming,
    }
    private AIAgent2D agent;
    private Order order;
    private VisitorState state = VisitorState.Idle;
    private WayPoint target = null;
    private float elapsedTime = 0f;

    void Awake(){
        agent = GetComponent<AIAgent2D>();
    }

    public void Enter(WayPoint waypoint){
        state = VisitorState.Arriving;
        target = waypoint;
        target.Reserve(gameObject);
        agent.SetDestination(waypoint.transform.position);
        agent.OnFinished += GenerateOrder;
    }
    public void Leave(){
        SetCoroutine(null);

        var queue = FindObjectOfType<VisitorQueue>();
        order = null;

        state = VisitorState.Leaving;
        target.Leave();
        target = null;
        agent.SetDestination(queue.doorLocation.position);
        agent.OnFinished += () => {
            Destroy(gameObject);
        };
    }

    private void GenerateOrder(){
        agent.OnFinished -= GenerateOrder;
        order = new Order(){
            duration = 4f,
        };
        SetCoroutine(AwaitingOrderCoroutine(order));
    }
    IEnumerator AwaitingOrderCoroutine(Order order){
        try{
            state = VisitorState.AwaitingOrder;
            progressIndicator.enabled = true;

            elapsedTime = 0f;
            while(elapsedTime < order.duration && order != null){
                elapsedTime += Time.deltaTime;
                progressIndicator.fillAmount = Mathf.Min(1f, elapsedTime / order.duration);
                yield return null;
            }
            Leave();
        }finally{
            progressIndicator.enabled = false;
        }
    }
    IEnumerator ConsumingCoroutine(){
        try{
            state = VisitorState.Consuming;
            bubbleParticles.Play();
            yield return new WaitForSeconds(5f);
            Leave();
        }finally{
            bubbleParticles.Stop();
        }
    }

    public override bool CanInteract(InteractionController interacting){
        if(state != VisitorState.AwaitingOrder) return false;
        var item = interacting.GetComponent<ItemHolder>()?.RetrieveItem(false);
        if(item == null) return false;
        //TODO check type
        return true;
    }

    public override void OnInteraction(InteractionController interacting){
        var item = interacting.GetComponent<ItemHolder>().RetrieveItem(true);
        SetCoroutine(ConsumingCoroutine());
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
