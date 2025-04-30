using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private Rigidbody2D _rigidBody;

    public int direction = 1;
    public float speed = 5;
    private BoxCollider2D _boxCollider;

    public float maxHealth;
    private float currentHealth;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(direction * speed, _rigidBody.velocity.y);
    }

    public void Death()
    {
        direction = 0;
        _rigidBody.gravityScale = 0;
        _boxCollider.enabled = false;
        Destroy(gameObject, 2);
    }

    public void TakeDamage(float damage)
    {
        currentHealth-= damage;
        
        if(currentHealth <= 0)
        {
            Death();
        }
        
    }

    void OnCollisionEnter2D(Collision2D Collision) 
    {
        if(Collision.gameObject.layer == 6 || Collision.gameObject.layer == 8)
        {
          direction *= -1;  
        }
    }

     void Movement()
    {
        if(direction > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if(direction < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
    }
}
