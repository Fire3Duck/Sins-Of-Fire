using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public bool isGrounded;

    private Rigidbody2D rigidBody;

    private PlayerControl playerScript;

    void Awake()
   {
     rigidBody = GetComponentInParent<Rigidbody2D>();

     playerScript = GetComponentInParent<PlayerControl>();
   }

    void OnTriggerEnter2D(Collider2D collider) 
   {
    if(collider.gameObject.layer == 3)
      {
         isGrounded  = true;
      }
      else if(collider.gameObject.layer == 6)
      {
        rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
      }
         
   }
      

    void OnTriggerStay2D(Collider2D collider) 
   {
      if(collider.gameObject.layer == 3)
    {
      isGrounded = true; 
    }
   }
   void OnTriggerExit2D(Collider2D collider) 
   {
     if(collider.gameObject.layer == 3)
    {
      isGrounded = false; 
    }
   }
}
