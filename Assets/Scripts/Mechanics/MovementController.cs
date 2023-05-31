using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour, InputActions.IPlayerActions {
    [field: SerializeField] private Animator animator;
    [field: SerializeField] private float movementSpeed = 0f;
    [field: SerializeField] private float accelerationDuration;
    [field: SerializeField] private float decelerationDuration;
    [field: SerializeField] private AnimationCurve movementCurve;
    [field: SerializeField] public ContactFilter2D collisionFilter;

    private Rigidbody2D rigidBody;
    private Vector2 direction;
    private float transition = 0f;

    private void Awake(){
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        bool stopping = direction.sqrMagnitude == 0f;

        transition = stopping
            ? Mathf.Max(0f, transition - Time.fixedDeltaTime / decelerationDuration)
            : Mathf.Min(1f, transition + Time.fixedDeltaTime / accelerationDuration);

        Vector3 velocity = stopping ? rigidBody.velocity : direction;
        velocity = velocity.normalized * Mathf.Lerp(0f, movementSpeed, movementCurve.Evaluate(transition));
        rigidBody.velocity = velocity;
  
        if(rigidBody.bodyType == RigidbodyType2D.Kinematic){
            rigidBody.MovePosition(rigidBody.position + rigidBody.velocity);
        }
    }

    void LateUpdate(){
        bool moving = rigidBody.velocity.sqrMagnitude >= 1e-3;

        animator.SetBool("Moving", moving);
        if(moving)
            transform.localScale = new Vector3(
                -Mathf.Sign(rigidBody.velocity.x), transform.localScale.y, transform.localScale.z
            );
    }

    public void OnMove(InputAction.CallbackContext context){
        direction = context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext context){}
}