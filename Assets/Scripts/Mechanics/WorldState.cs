using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/WorldState")]
public class WorldState : GameManager.AbstractWorldState {
    [field: SerializeField] public float WorkingPhaseDuration { get; private set; }

    public enum WorldPhase {
        Working, Resting,
    }
    private WorldPhase phase = WorldPhase.Resting;
    private double startTime = 0.0;

    public WorldPhase Phase => phase;
    public float Elapsed => (float)(Time.timeAsDouble - startTime);

    public override IEnumerator Play(){
        while(true){
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
        startTime = Time.timeAsDouble;
    }
}
