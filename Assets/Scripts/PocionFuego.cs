using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocionFuego : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private AudioSource _audioSource;
    public AudioClip manaSFX;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerControl = GameObject.Find("Personaje").GetComponent<PlayerControl>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            _playerControl.RestoreMana();
            _audioSource.PlayOneShot(manaSFX);
            Death();
        }
    }

    void Death()
    {
        _spriteRenderer.enabled = false;
        _boxCollider.enabled = false;
        Destroy(gameObject, 1.5f);
    }
}
