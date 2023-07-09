using UnityEngine;
using CleverCrow.Fluid.Dialogues;

[CreateAssetMenu(menuName = "Gameplay/Character")]
public class Character : ActorDefinition {
    [HideInInspector] public bool active;
	
	void OnEnable(){
		active = true;
	}

    
}
