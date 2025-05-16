using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
private SoundManager _soundManager;
public bool isPlaying = true;

    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
