using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/WorldState")]
public class WorldState : GameManager.AbstractWorldState {
    [field: SerializeField] public float WorkingPhaseDuration { get; private set; }

    public enum WorldPhase {
        Working, Resting,
    }
    public float globalTimeScale;
    private WorldPhase phase = WorldPhase.Resting;
    private double elapsed = 0.0;

    public WorldPhase Phase => phase;
    public float Elapsed => (float) elapsed;

    public override IEnumerator Play(){
        phase = WorldPhase.Resting;
        globalTimeScale = 1.0f;
        while(true){
            elapsed += globalTimeScale * Time.deltaTime;
            switch(phase){
                case WorldPhase.Working:
                    if(Elapsed >= WorkingPhaseDuration) StartPhase(WorldPhase.Resting);
                    goto case WorldPhase.Resting;
                case WorldPhase.Resting:
                    break;
                default:
                    yield break;
            }
            yield return null;
        }
    }

    public event UnityAction<WorldPhase> OnPhaseTransition;

    public void StartPhase(WorldPhase nextPhase){
        OnPhaseTransition?.Invoke(nextPhase);
        phase = nextPhase;
        elapsed = 0.0;
    }

    //TODO remove
    public static WorldState instance;
    void Awake(){ WorldState.instance = this; }
    void OnEnable(){ WorldState.instance = this; }
}
