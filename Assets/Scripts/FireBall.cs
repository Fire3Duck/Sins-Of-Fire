using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public float fireForce = 10;

    public float fireDamage = 2;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody.AddForce(transform.right * fireForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == 7)
        {
            Enemy enemyScript = collider.gameObject.GetComponent<Enemy>();
            enemyScript.TakeDamage(fireDamage);
            Debug.Log("Hola");
            FireDeath(); 
        }

        if(collider.gameObject.layer == 3)
        {
            FireDeath();
        }
    }

    void FireDeath()
    {
        Destroy(gameObject);
    }
}
