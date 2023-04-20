using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private Sprite redBeerGlassSprite;
    [SerializeField] private Sprite greenBeerGlassSprite;
    [SerializeField] private Sprite greenBeerBarrelSprite;
    [SerializeField] private Sprite redBeerBarrelSprite;
    [SerializeField] private Sprite cleaningMaterialsSprite;
    [SerializeField] private Sprite redBeerIngredientSprite;
    [SerializeField] private Sprite greenBeerIngredientSprite;
    [SerializeField] private Image currentItem;
    [SerializeField] private GameObject rabbitSprite;
    [SerializeField] private RectTransform itemRectTransform;

    public CharacterController controller;
    public PlayerInputActions playerInputActions;
    public Transform mainCamera;

    private InputAction movement;
    private InputAction fire;
    private InputAction interaction;

    private bool attackBlocked;
    private GameObject target;
    private Vector2 moveDirection = Vector2.zero;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private int currentBeerType;
    private int playerSpeed;

    private Vector3 rabbitGameobjectScale;
    private Animator rabbitAnimator;

    private Vector3 itemScale;
    private Vector3 itemPosition;

    private bool isRabbitLookingLeft = true;

    public bool isHoldingBeerKeg;
    public bool isHoldingGlassOfBeer;
    public bool isHoldingCleaningMaterials;
    public bool isHoldingBeerIngredient;

    public int CurrentBeerType { get { return currentBeerType; } }
    public int PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rabbitAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        playerSpeed = GameConfigManager.PlayerSpeed;

    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Move;
        fire = playerInputActions.Player.Fire;
        fire.performed += Fire;
        interaction = playerInputActions.Player.Interact;
        interaction.performed += Interact;

        interaction.Enable();
        fire.Enable();
        movement.Enable();
      
        currentItem.enabled = false;

        TavernEventsManager.OnDayStarted += DayStartsHandler;
        TavernEventsManager.OnNightStarted += NightStartsHandler;
        TavernEventsManager.OnSpeedImproved += SpeedImprovedHandler; 

        isHoldingBeerKeg = false;
        isHoldingGlassOfBeer = false;
        isHoldingCleaningMaterials = false;
        isHoldingBeerIngredient = false;

        itemScale = itemRectTransform.localScale;
        rabbitGameobjectScale = rabbitSprite.transform.localScale;
    }

private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
        interaction.Disable();

        TavernEventsManager.OnDayStarted -= DayStartsHandler;
        TavernEventsManager.OnNightStarted -= NightStartsHandler;
        TavernEventsManager.OnSpeedImproved -= SpeedImprovedHandler;
    }

    void Update()
    {
        moveDirection = movement.ReadValue<Vector2>();
        SideHandler(moveDirection.x);
        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            rabbitAnimator.SetBool("Moving", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            //controller.Move(direction * GameConfigManager.PlayerSpeed * Time.deltaTime);
        } else
        {
            rabbitAnimator.SetBool("Moving", false);
        }
    }

    private void SideHandler(float xValue)
    {
       if(xValue > 0 && isRabbitLookingLeft)
        {
            FlipRabbitSprite();
        }
       else if(xValue < 0 && !isRabbitLookingLeft)
        {
            FlipRabbitSprite();
        }

    }

    private void FlipRabbitSprite()
    {
        isRabbitLookingLeft = !isRabbitLookingLeft;
        rabbitGameobjectScale.x *= -1;
        rabbitSprite.transform.localScale = rabbitGameobjectScale;
        itemScale.x *= -1;
        itemRectTransform.localScale = itemScale;
        itemPosition = itemRectTransform.localPosition;
        itemPosition.x *= -1;
        itemRectTransform.localPosition = itemPosition;
    }

    private void NightStartsHandler()
    {
        movement.Disable();
        fire.Disable();
        interaction.Disable();
    }

    private void DayStartsHandler()
    {
        interaction.Enable();
        fire.Enable();
        movement.Enable();
    }

    public void TakeBeerKeg(int beerType)
    {
        currentItem.enabled = true;
        currentItem.sprite = beerType == 1 ? greenBeerBarrelSprite : redBeerBarrelSprite;
        isHoldingBeerKeg = true;
        //kegOfBeer.SetActive(true);
        currentBeerType = beerType;
        rabbitAnimator.SetBool("Carrying", true);
    }

    public void ReleaseBeerKeg()
    {
        isHoldingBeerKeg = false;
        //kegOfBeer.SetActive(false);
        currentItem.enabled = false;
        rabbitAnimator.SetBool("Carrying", false);
    }

    public void TakeGlassOfBeer(int beerType)
    {
        currentItem.enabled = true;
        currentItem.sprite = beerType == 1 ? greenBeerGlassSprite : redBeerGlassSprite;
        isHoldingGlassOfBeer = true;
        rabbitAnimator.SetBool("Carrying", true);

    }

    public void ReleaseGlassOfBeer()
    {
        isHoldingGlassOfBeer = false;
        currentItem.enabled = false;
        rabbitAnimator.SetBool("Carrying", false);
    }

    public void TakeCleaningMaterials()
    {
        isHoldingCleaningMaterials = true;
        currentItem.sprite = cleaningMaterialsSprite;
        currentItem.enabled = true;
        rabbitAnimator.SetBool("Carrying", true);
    }

    public void ReleaseCleaningMaterials()
    {
        isHoldingCleaningMaterials = false;
        currentItem.enabled = false;
        rabbitAnimator.SetBool("Carrying", false);
    }

    public void TakeBeerIngredient(int beerType)
    {
        currentBeerType = beerType;
        isHoldingBeerIngredient = true;
        currentItem.sprite = currentBeerType == 1 ? greenBeerIngredientSprite : redBeerIngredientSprite;
        currentItem.enabled = true;
        rabbitAnimator.SetBool("Carrying", true);
    }

    public void ReleaseBeerIngredient()
    {
        isHoldingBeerIngredient = false;
        currentItem.enabled = false;
        rabbitAnimator.SetBool("Carrying", false);
    }

    public void ReleaseAnyItem()
    {
        if (isHoldingCleaningMaterials)
        {
            ReleaseCleaningMaterials();
        }
        if (isHoldingBeerKeg)
        {
            ReleaseBeerKeg();
        }
        if (isHoldingGlassOfBeer)
        {
            ReleaseGlassOfBeer();
        }
        if (isHoldingBeerIngredient)
        {
            ReleaseBeerIngredient();
        }
        rabbitAnimator.SetBool("Carrying", false);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        print(target);
    }
  
    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (attackBlocked || target == null)
            {
                return;
            }
            Debug.Log("Attack");
            attackBlocked = true;
            StartCoroutine(DelayAttack());
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && target != null)
        {
            if(target.TryGetComponent(out PlayerInteractable interactable))
            {
                interactable.PlayerInteraction();
            }
        }
        else
        {
            Debug.Log("No Target to interract");
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }

    private void SpeedImprovedHandler()
    {
        playerSpeed++;
        print("player speed " + playerSpeed);
    }
}
