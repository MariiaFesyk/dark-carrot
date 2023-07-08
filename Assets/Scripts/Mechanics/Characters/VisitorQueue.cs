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
            available = QueryAvailable();
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

    public Item[] available;

    public Item[] QueryAvailable(){
        var sources = FindObjectsByType<Dispenser>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var transforms = FindObjectsByType<CraftingDevice>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        var set = new Dictionary<Item, bool>();
        foreach(var raw in sources) set.Add(raw.Item, true);

        var recipes = new List<CraftingDevice.CraftingRecipe>();
        foreach(var transform in transforms)
            foreach(var recipe in transform.recipes) recipes.Add(recipe);

        //TODO optimize naive approach
        while(true){
            int revealed = 0;
            for(int i = recipes.Count - 1; i >= 0; i--){
                var recipe = recipes[i];
                var available = System.Array.TrueForAll(recipe.inputList, item => set.ContainsKey(item));
                if(available){
                    set.Add(recipe.output, true);
                    recipes.RemoveAt(i);
                    revealed++;
                }
            }
            if(revealed == 0) break;
        }

        Item[] items = new Item[set.Keys.Count];
        set.Keys.CopyTo(items, 0);
        return items;
    }
}