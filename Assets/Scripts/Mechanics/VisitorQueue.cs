using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorQueue : MonoBehaviour {
    [SerializeField] private GameObject[] visitors;
    [SerializeField] public Transform doorLocation;
    [SerializeField] private WorldState state;
    [SerializeField] private GameObject layer;
	[SerializeField] private Vector2 spawnRate;
    private List<Visitor> activeVisitors = new();
    private Coroutine coroutine = null;

    void OnEnable(){
        state.OnPhaseTransition += OnPhaseTransition;
    }
    void OnDisable(){
        state.OnPhaseTransition -= OnPhaseTransition;
    }
    void OnPhaseTransition(WorldState.WorldPhase phase){
        if(phase == WorldState.WorldPhase.Working && coroutine == null){
            coroutine = StartCoroutine(SpawnVisitorsCoroutine());
        }else if(phase != WorldState.WorldPhase.Working && coroutine != null){
            StopCoroutine(coroutine);
            coroutine = null;
            foreach(var visitor in activeVisitors)
                visitor.Leave();
        }
    }
    private IEnumerator SpawnVisitorsCoroutine(){
        while(true){
            float interval = Random.Range(spawnRate.x, spawnRate.y);
            yield return new WaitForSeconds(interval);

            if(visitors.Length == 0) continue;
            GameObject prefab = visitors[Random.Range(0, visitors.Length)];

            //TODO remove linq
            var tables = FindObjectsOfType<WayPoint>().Where(table => table.IsEmpty && table.Contains("seat")).ToArray();
            if(tables.Length == 0) continue;
            var table = tables[Random.Range(0, tables.Length)];

            var visitor = Instantiate(prefab, doorLocation.position, Quaternion.identity, layer.transform);
            visitor.GetComponent<Visitor>().Enter(table);
            activeVisitors.Add(visitor.GetComponent<Visitor>());
        }
    }
}