using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeloRedirrector : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 desiredVelo;
    public float timeWait = .5f;
    public float factor = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        desiredVelo = new Vector2(rb.linearVelocity.y, -rb.linearVelocity.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeWait > 0)
        {
            timeWait -= Time.deltaTime;
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVelo, factor * Time.deltaTime);
        }
    }
}
