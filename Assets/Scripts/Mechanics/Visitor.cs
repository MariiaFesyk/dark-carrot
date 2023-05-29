using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AIAgent2D))]
public class Visitor : Interactable {
    [SerializeField] private ParticleSystem bubbleParticles;
    [SerializeField] private Image progressIndicator;

    public enum VisitorState {
        Idle, Arriving, Leaving, AwaitingOrder, Consuming,
    }
    private AIAgent2D agent;
    private VisitorState state = VisitorState.Idle;
    private WayPoint target = null;
    private float elapsedTime = 0f;

    void Awake(){
        agent = GetComponent<AIAgent2D>();
    }

    public void Enter(WayPoint waypoint){
        target = waypoint;
        state = VisitorState.Arriving;
        target.Reserve(gameObject);
        agent.SetDestination(waypoint.transform.position);
    }

    void Update(){
        switch(state){
            case VisitorState.Arriving:
                if(!agent.IsMoving) state = VisitorState.AwaitingOrder;
                break;
            case VisitorState.Idle:
            default:
                break;
        }
    }
    void OnDestroy(){

    }

    public override bool CanInteract(InteractionController interacting){
        return false;
    }

    public override void OnInteraction(InteractionController interacting){
        
    }
}
