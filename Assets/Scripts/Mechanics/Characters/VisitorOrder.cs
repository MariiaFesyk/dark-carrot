using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Order", fileName = "New Order")]
public class VisitorOrder : ScriptableObject {
    [field: SerializeField, Min(0f)] public float Duration { get; private set; }

    [SerializeField] private Tag[] tags;
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

        foreach(var tag in tags) if(System.Array.IndexOf(item.Tags, tag) == -1) return false;

        return true;
    }

    public void Fulfill(Item item, Visitor visitor){
        foreach(var reward in rewards){
            reward.resource.Amount += reward.amount + reward.multiplier * item.cost;
        }
    }
}
