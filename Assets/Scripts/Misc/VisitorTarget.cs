using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisitorTarget : MonoBehaviour
{
    [SerializeField] protected Transform targetPoint;
    protected VisitorAI visitor;

    public Transform TargetPoint { get { return targetPoint; } }

    public abstract void OnTargetReachedHandler();
}
