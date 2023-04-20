
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] bool freezeXZAxis = true;

    private Transform mainCamera; 

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }


    private void Update()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, mainCamera.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = mainCamera.rotation;
        }
    }
}
