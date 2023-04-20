using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float xScrollSpeed = 0.1f;
    [SerializeField] private float yScrollSpeed = 0.1f;


    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float xOffset = Time.time * xScrollSpeed;
        float yOffset = Time.time * yScrollSpeed;

        rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
    }
}
