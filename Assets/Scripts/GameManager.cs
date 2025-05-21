using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private SoundManager _soundManager;
    public bool isPlaying = true;

    public Text diamondText;

    private int diamond = 0;
    void Start()
    {
        diamondText.text = diamond.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    public void AddDiamond()
    {
        diamond++; 
        diamondText.text = diamond.ToString();
        //para que cuente monedas de uno en uno.
    } 
    // Start is called before the first frame update
    
}
