using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofres : MonoBehaviour
{
    public bool isChestOpen = false;
    public GameObject _diamantes;
    public Transform _spawner;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _cofreSFX;
    [SerializeField] private Animator _animator;


    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isChestOpen == true)
        {
            return;
        }
    }

    public void OpenChest()
    {
        if(isChestOpen == false)
        {
            _audioSource.PlayOneShot(_cofreSFX);
            isChestOpen = true;
            _animator.SetBool("IsOpen", true);
            Instantiate(_diamantes, _spawner.position, _spawner.rotation);
        }
       
    }
}
