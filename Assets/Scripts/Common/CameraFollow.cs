using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField, Range(0.0f, 10.0f)] private float smoothTime = 0.0f;
    [SerializeField] private float maxSpeed = float.MaxValue;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate(){
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position + offset,
            ref velocity,
            smoothTime,
            maxSpeed,
            Time.deltaTime
        );
    }
}
