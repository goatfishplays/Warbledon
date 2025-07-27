using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    private float speed = 0;
    private float rotation = 0;

    public Vector2 startRotationBounds;
    public Vector2 rotationSpeedBounds;

    void Start()
    {
        rotation = Random.Range(startRotationBounds.x, startRotationBounds.y);
        speed = Random.Range(rotationSpeedBounds.x, rotationSpeedBounds.y);
    }

    // Update is called once per frame
    void Update()
    {
        rotation += speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
