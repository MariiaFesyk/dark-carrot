using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorQueue : MonoBehaviour {
    [SerializeField] public Phase phase;
    [SerializeField] private Visitor[] commonVisitors;
    [SerializeField] private Visitor[] uniqueVisitors;

    [SerializeField] public Transform doorLocation;
    [SerializeField] private Transform parent;

    [Header("Common")]
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;

    void OnEnable(){
        phase.OnPhaseEnter += OnPhaseTransition;
        phase.OnPhaseExit += OnPhaseTransition;
    }
    void OnDisable(){
        phase.OnPhaseEnter -= OnPhaseTransition;
        phase.OnPhaseExit -= OnPhaseTransition;
    }
    void OnPhaseTransition(Phase phase){
        StopAllCoroutines();
        var visitors = FindObjectsOfType<Visitor>(false);
        foreach(var visitor in visitors){
            if(!phase.enabled) visitor.OnWorkingPhaseEnd();
            else visitor.Leave();
        }

        if(phase.enabled){
            StartCoroutine(SpawnCommonVisitors(commonVisitors));
            StartCoroutine(SpawnUniqueVisitors(uniqueVisitors));
        }
    }

    private IEnumerator SpawnUniqueVisitors(Visitor[] visitors){
        visitors = System.Array.FindAll(visitors, visitor => visitor.schedule.Validate());
        visitors.Shuffle();
        List<(Visitor, float)> queue = new();
        foreach(var visitor in visitors){
            float arrivalTime = Random.Range(0.1f, 0.25f) * phase.duration;
            queue.Add((visitor, arrivalTime));
        }

        while(queue.Count > 0){
            yield return null;
            var (prefab, time) = queue[0];
            if(phase.elapsed < time) continue;
            var table = QueryRandomSeat(true);
            if(!table) continue;

            queue.RemoveAt(0);

            var visitor = Instantiate(prefab, doorLocation.position, Quaternion.identity, parent);
            visitor.Enter(table);
        }
    }

    private IEnumerator SpawnCommonVisitors(Visitor[] visitors){
        visitors = System.Array.FindAll(visitors, visitor => visitor.schedule.Validate());

        if(visitors.Length == 0) yield break;
        while(true){
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
            if(phase.timeScale == 0f) continue;
            // yield return new WaitUntil(() => phase.timeScale > 0f);

            var prefab = visitors[Random.Range(0, visitors.Length)];
            var table = QueryRandomSeat(false);
            if(!table) continue;

            var visitor = Instantiate(prefab, doorLocation.position, Quaternion.identity, parent);
            visitor.Enter(table);
        }
    }

    private SeatSpot QueryRandomSeat(bool unique){
        var candidates = FindObjectsOfType<SeatSpot>();
        List<SeatSpot> options = new();
        foreach(var candidate in candidates)
            if(candidate.IsEmpty && candidate.Contains(unique ? "vip_seat" : "seat")){
                options.Add(candidate);
            }
        if(options.Count == 0) return null;
        return options[Random.Range(0, options.Count)];
    }
}