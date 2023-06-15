using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorQueue : MonoBehaviour {
    [SerializeField] private GameObject[] visitors;

    [System.Serializable]
    public class CharacterVisitor { //TODO move out to scriptable object
        public GameObject prefab;
    }

    [SerializeField] private CharacterVisitor[] characters;
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
        float arriveAt = Random.Range(0.1f, 0.25f) * state.WorkingPhaseDuration * 0f;
        if(characters.Length == 0) arriveAt = -1f;


        while(true){
            float interval = Random.Range(spawnRate.x, spawnRate.y);
            yield return new WaitForSeconds(interval);

            if(state.globalTimeScale == 0f) continue;


            if(arriveAt < state.Elapsed && arriveAt >= 0f){
                arriveAt = -1f;
                var vip_tables = FindObjectsOfType<WayPoint>().Where(table => table.IsEmpty && table.Contains("vip_seat")).ToArray();
                if(vip_tables.Length > 0){
                    var vip_visitor = Instantiate(characters[0].prefab, doorLocation.position, Quaternion.identity, layer.transform);
                    vip_visitor.GetComponent<Visitor>().Enter(vip_tables[0]);
                    activeVisitors.Add(vip_visitor.GetComponent<Visitor>());
                }
            }


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