using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AIAgent2D))]
public class Visitor : Interactable {
    [SerializeField] private ParticleSystem bubbleParticles;
    [SerializeField] private Image progressIndicator;
    [SerializeField] private Image orderIcon;
    [SerializeField] private Order[] orders;

    [System.Serializable]
    public class Order {
        public Tag[] tags;
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
        if(target == null) return;
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
        order = orders[Random.Range(0, orders.Length)];
        SetCoroutine(AwaitingOrderCoroutine(order));
    }
    IEnumerator AwaitingOrderCoroutine(Order order){
        try{
            orderIcon.transform.parent.gameObject.SetActive(true);
            orderIcon.sprite = order.tags[0]?.Icon;

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
            orderIcon.transform.parent.gameObject.SetActive(false);
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
        var item = interacting.GetComponent<ItemHolder>()?.Item;
        if(item == null) return false;
        //TODO check type
		
		foreach(var tag in order.tags)
			if(System.Array.IndexOf(item.Tags, tag) == -1) return false;
		
        return true;
    }

    public override void OnInteraction(InteractionController interacting){
        var item = interacting.GetComponent<ItemHolder>().RetrieveItem();
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
