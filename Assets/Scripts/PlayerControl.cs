using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

private Rigidbody2D rigidBody2D;
private Animator _animator;
private GroundSensor _groundSensor;
private SpriteRenderer _spriteRender;
private BoxCollider2D _boxCollider;

public float inputHorizontal;

public float jumpForce = 10;

public float Velocity = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && _groundSensor.isGrounded == true) //GetButton tiene 3 formas (El Down que hace la accion cuando pulsa el boton, el Up que hace la acicon cuando sueltas el boton y el GetButton que hace manteniendo el boton)
        {
            Jump();
        }

        Movement();

        _animator.SetBool("IsJumping", !_groundSensor.isGrounded);
    }

    void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(inputHorizontal * Velocity, rigidBody2D.velocity.y);
    }

    void Movement ()
    {
        if(inputHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if(inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

    void Jump()
    {
    rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //hacer que salte
    _animator.SetBool("IsJumping", true);
    }
}
