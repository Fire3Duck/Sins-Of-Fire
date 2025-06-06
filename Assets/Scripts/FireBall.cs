using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public float fireForce = 10;

    public float fireDamage = 2;

    private Animator _animator;
    private bool Fire = true;

    private SoundManager _soundManager;
    

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody.AddForce(transform.right * fireForce, ForceMode2D.Impulse);
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == 7) //El numero es el Layer.En este caso el 7 son los enemigos.
        {
            Enemy enemyScript = collider.gameObject.GetComponent<Enemy>(); //Enemy es el nombre del script.
            enemyScript.TakeDamage(fireDamage);
            Debug.Log("Hola");
            FireDeath(); 
        }

        if(collider.gameObject.layer == 3) //El numero es el suelo.
        {
            FireDeath();
        }
    }

    void FireDeath()
    {
        Destroy(gameObject);
        _animator.SetBool("Fire", true);
    }
}
