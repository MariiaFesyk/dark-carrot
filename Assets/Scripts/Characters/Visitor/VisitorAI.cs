using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class VisitorAI : MonoBehaviour
{
    [SerializeField] private GameObject type1Sprite;
    [SerializeField] private GameObject type2Sprite;
    [SerializeField] private GameObject type3Sprite;
    [SerializeField] private ParticleSystem bubblesParticle;

    private Transform target;
    private VisitorTargets currentTargetType;
    private Table occupiedTable;
    private CharacterController characterController;
    private int strength = 1;
    private float targetThreshold = 0.2f;
    [SerializeField] private int visitorType;
    private int beerOrderType;

    public int Strength => strength;
    public int CurrentType => visitorType;
    public Table OccupiedTable => occupiedTable;
    public int BeerOrderType => beerOrderType;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = false;
    }
    private void Update()
    {
        if(target)
        {
            MoveToTarget();
        }
    }
   
    private void MoveToTarget()
    {
        if (target)
        {
            Vector3 targetDirection = (target.transform.position - this.transform.position).normalized;
            characterController.Move(new Vector3(targetDirection.x, 0f, targetDirection.z) * GameConfigManager.VisitorSpeed * Time.deltaTime);
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < targetThreshold) 
            {
                ReachTargetHandler();
            }
        }
    }

    private void ReachTargetHandler()
    {
        switch (currentTargetType)
        {
            case VisitorTargets.Table:
                occupiedTable.VisitorReachTheTable();
                break;
            case VisitorTargets.Door:
                TavernEventsManager.VisitorLeftTavern(gameObject);
                break;
            default:
                break;
        }
        target = null;
    }

    public void SetTarget(Transform newTarget, VisitorTargets targetType)
    {
        target = newTarget;
        currentTargetType = targetType;
        if(targetType == VisitorTargets.Table)
        {
            occupiedTable = newTarget.GetComponentInParent<Table>();
        }
    }

    public void BubblesToogle()
    {
        if (bubblesParticle.isPlaying)
        {
            bubblesParticle.Stop();
        } else
        {
            bubblesParticle.Play();
        }

    }

    public void SetStats(int strength, int visitorType, int firstOrderBeerType)
    {
        this.strength = strength;
        this.visitorType = visitorType;
        this.beerOrderType = firstOrderBeerType;
        SpriteChangeHandler(this.visitorType);
    }

    private void SpriteChangeHandler(int visitorType)
    {
        switch (visitorType)
        {
            case 1:
                type1Sprite.SetActive(true);
                type2Sprite.SetActive(false);
                type3Sprite.SetActive(false);
                break;
            case 2:
                type1Sprite.SetActive(false);
                type2Sprite.SetActive(true);
                type3Sprite.SetActive(false);
                break;
            case 3:
                type1Sprite.SetActive(false);
                type2Sprite.SetActive(false);
                type3Sprite.SetActive(true);
                break;
            default:
                type1Sprite.SetActive(true);
                type2Sprite.SetActive(false);
                type3Sprite.SetActive(false);
                break;
        }
    }

    public void OnBeerDrinkEffect()
    {
        AddStrength(GameConfigManager.OnBeerDrinkStrengthReward);
        beerOrderType = beerOrderType == 1 ? 2 : 1;
    }

    public void OnCarrotEatEffect()
    {
        AddStrength(GameConfigManager.OnCarrotEatStrengthReward);
    }

    public void AddStrength(int valueToAdd)
    {
        strength += valueToAdd;
    }

}

