using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

private Rigidbody2D rigidBody2D;

public float Horizontal;

public float Velocity = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(Horizontal * Velocity, rigidBody2D.velocity.y);
    }
}
