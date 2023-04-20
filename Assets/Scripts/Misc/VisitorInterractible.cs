using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public abstract class VisitorInterractible : MonoBehaviour
{
    protected VisitorAI visitor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            Debug.Log(visitor.gameObject.name);
            visitor = other.GetComponent<VisitorAI>();
            VisitorInterraction(visitor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Visitor"))
        {
            

        }
    }
    public abstract void VisitorInterraction(VisitorAI visitor);
}

