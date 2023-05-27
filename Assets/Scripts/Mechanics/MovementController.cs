using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour, InputActions.IPlayerActions
{
    [field: SerializeField] private Animator animator;
    [field: SerializeField] private float movementSpeed = 0f;
    [field: SerializeField] public ContactFilter2D collisionFilter;

    private Rigidbody2D _rigidBody;
    private Vector2 _direction;

    private void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){ 
        _rigidBody.velocity = _direction.normalized * movementSpeed;
  
        if(_rigidBody.bodyType == RigidbodyType2D.Kinematic){
            _rigidBody.MovePosition(_rigidBody.position + _rigidBody.velocity);
        }

        animator.SetBool("Moving", _rigidBody.velocity.sqrMagnitude >= 1e-3);
        if(_rigidBody.velocity.x > 0.0) transform.localScale = new Vector3(-1.0f, transform.localScale.y, transform.localScale.z);
        else if(_rigidBody.velocity.x < 0.0) transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);
    }

    public void OnMove(InputAction.CallbackContext context){
        _direction = context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext context){}
}