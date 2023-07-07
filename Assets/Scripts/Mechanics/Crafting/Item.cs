using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Item", fileName = "New Item")]
public class Item : ScriptableObject {
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

	[field: SerializeField] public int cost;

    [field: SerializeField] public Tag[] Tags { get; private set; }
	
	public Tag VariantTag {
		get {
			foreach(var tag in Tags)
				if(tag.isVariant) return tag;
			return null;
		}
	}

	[System.Serializable]
	public struct ItemFilter {
		public Tag[] tags;
		public Item[] whitelist;

		public bool Validate(Item item){
			if(item == null) return false;
			if(System.Array.IndexOf<Item>(whitelist, item) != -1) return true;

			if(tags.Length == 0) return true;
			foreach(var tag in tags)
				if(System.Array.IndexOf<Tag>(item.Tags, tag) != -1) return true;
			return false;
		}
	}
}
