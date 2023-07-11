using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Order", fileName = "New Order")]
public class VisitorOrder : ScriptableObject {
    [field: SerializeField, Min(0f)] public float Duration { get; private set; }

    //TODO use filter
    [SerializeField] private Tag[] tags;
    [SerializeField] private Item[] items;
    [SerializeField] private Sprite icon;
    [SerializeField] private Reward[] rewards;

    public Sprite Icon => icon != null ? icon : tags[0]?.Icon;

    [System.Serializable]
    public class Reward {
        public Resource resource;
        public int amount;
        public int multiplier;
    }

    public bool Validate(Item item){
        if(item == null) return false;

        if(items != null) foreach(var i in items) if(i == item) return true;

        foreach(var tag in tags) if(System.Array.IndexOf(item.Tags, tag) == -1) return false;

        return true;
    }

    public void Fulfill(Item item, Visitor visitor){
        //TODO inject some other way? emit an global scriptable event that log will be listening to?
        var log = FindFirstObjectByType<AwardLog>();

        foreach(var reward in rewards){
            int amount = reward.amount + reward.multiplier * item.cost;
            reward.resource.Amount += amount;
            log?.ShowReward(visitor.gameObject, reward.resource, amount);
        }
    }
}
