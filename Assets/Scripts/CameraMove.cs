using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform playerTransform; //seleccionar el personaje.

    public Vector3 offset; //para modificar la camara. tocar la Z.
    public Vector2 maxPosition;
    public Vector2 minPosition;

    public float interpolationRatio = 0.5f;


    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; //Busca un objeto por el Tag.
    }

void FixedUpdate()
    {
        if(playerTransform == null)
        {
            return;
        }
        
        Vector3 desiredPosition = playerTransform.position + offset; //Mover la camara
        
         //Para poner un limite en la camara
         float clampX = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
         float clampY = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
         Vector3 clampedPosition = new Vector3(clampX, clampY, desiredPosition.z);

         Vector3 lerpedPosition = Vector3.Lerp(transform.position, clampedPosition, interpolationRatio);

         transform.position = lerpedPosition;
    }

}
