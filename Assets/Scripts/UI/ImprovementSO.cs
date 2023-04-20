using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Improvement", menuName = "Improvement")]
public class ImprovementSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemEffect;
    [TextArea] [SerializeField] private string itemDescription;
    [SerializeField] private int itemPrice;
    [SerializeField] private Sprite icon;
    [SerializeField] [Range(0, 3)] private int stars;
    public UnityEvent effect;


    public string ItemName { get { return itemName; } }
    public string ItemEffect { get { return itemEffect; } }
    public string ItemDescription { get { return itemDescription; } }
    public int ItemPrice { get { return itemPrice; } }
    public Sprite Icon { get { return icon; } }
    public int Stars { get { return stars; } }
    public UnityEvent Effect { get { return effect; } }



}
