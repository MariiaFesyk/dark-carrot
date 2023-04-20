using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDoor : MonoBehaviour
{
    [SerializeField] private Transform visitorTargetPoint;

    public Transform VisitorTargetPoint { get { return visitorTargetPoint; } }
}
