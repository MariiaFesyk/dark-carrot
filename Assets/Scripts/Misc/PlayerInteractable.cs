using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Outline))]

public abstract class PlayerInteractable : MonoBehaviour
{
    protected PlayerController player;
    protected Outline outline;
    protected bool isInteractable;


    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
        outline.OutlineColor = Color.green;
        isInteractable = true;
        //#678873
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            player = other.GetComponent<PlayerController>();
            player.SetTarget(gameObject);
            outline.OutlineWidth = 2.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            player.SetTarget(player.gameObject);
            outline.OutlineWidth = 0;
        }
    }

    public abstract void PlayerInteraction();
}
