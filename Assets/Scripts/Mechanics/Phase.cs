using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/Phase", fileName = "New Phase")]
public class Phase : ScriptableObject {
    public static Phase last;

    [SerializeField] private string title;
    [SerializeField] public float duration;
    [SerializeField] private Phase nextPhase;

    [HideInInspector] public bool enabled = false;
    public float timeScale = 1f;
    public float elapsed { get; private set; } = 0f;
    public int count { get; private set; } = 0;

    public float deltaTime => enabled ? Time.deltaTime * timeScale : 0f;
    public float percent => Mathf.Clamp01(elapsed / duration);

    public event UnityAction<Phase> OnPhaseEnter;
    public event UnityAction<Phase> OnPhaseExit;

    void OnEnable(){
        enabled = false;
    }

    public void Enter(){
        count++;
        elapsed = 0f;
        enabled = true;
        last = this;
        OnPhaseEnter?.Invoke(this);
    }

    public void Exit(){
        enabled = false;
        OnPhaseExit?.Invoke(this);

        if(nextPhase != null) nextPhase.Enter();
    }

    public void Update(){
        if(!enabled) return;
        elapsed += deltaTime;
        if(duration > 0f && elapsed >= duration) Exit();
    }
}
