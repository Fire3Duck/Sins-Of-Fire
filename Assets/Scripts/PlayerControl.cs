using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

private Rigidbody2D rigidBody;
public Animator _animator;
private GroundSensor _groundSensor;
private SpriteRenderer _spriteRender;
private BoxCollider2D _boxCollider;
private GameManager _gameManager;
private SoundManager _soundManager;


//El Dash
[SerializeField] private float _dashForce = 20;
[SerializeField] private float _dashDuration = 0.5f;
[SerializeField] private float _dashCoolDown = 1;
private bool _canDash = true;
private bool _isDashing = false;

//El ataque
[SerializeField] private LayerMask _enemyLayer;
[SerializeField] private float _attackDamage = 10;
[SerializeField] private float _attackRadius = 1;
[SerializeField] private Transform _hitBoxPosition;

//Ataque cargado
[SerializeField] private float _baseChargedAttackDamage = 15;
[SerializeField] private float _maxChargedAttackDamage = 40;
private float _chargedAttackDamage;
private bool _isCharging = false;

//Que se escuche cuando camine
[SerializeField] private AudioSource _audioSource;
[SerializeField] private AudioClip _footStepsAudio;
private bool _alreadyPlaying = false;

//Disparo
public Transform fireSpawn;
public GameObject firePrefab;
public bool canShoot = true;

private bool _hasMana = true;


[SerializeField] private float _fireballCost = 0.35f;

//Sonido ataque
[SerializeField] private AudioClip _atackAudio;

public AudioClip deathSFX;
public AudioClip jumpSFX;
public AudioClip shootSFX;
public AudioClip dashSFX;

//Vida
[SerializeField] private float _currentHealth;
[SerializeField] private float _maxHealth = 1;
[SerializeField] private Image _healthBar;
[SerializeField] private AudioClip _damage;
[SerializeField] private SpriteRenderer _spriteRenderer;

public float inputHorizontal;

public float jumpForce = 10;

public float Velocity = 4.5f;

//Particulas
private ParticleSystem _particleSystem;
private Transform _particlesTransform;
private Vector3 _particlesPosition;

public Cofres _chests;
public bool _IsChestHere;

//Mana
[SerializeField] private float _currentMana;
[SerializeField] private float _maxMana = 1;
[SerializeField] private Image _manaBar;


    // Start is called before the first frame update
    void Start()
    {
        _chargedAttackDamage = _baseChargedAttackDamage;
        _audioSource.loop = true;
        _audioSource.clip = _footStepsAudio;

        _currentMana = _maxMana;
        _currentHealth = _maxHealth;
        _healthBar.fillAmount = _maxHealth;
        _manaBar.fillAmount = _maxMana;
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particlesTransform = _particleSystem.transform;
        _particlesPosition = _particlesTransform.localPosition;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _manaBar = GameObject.Find("ManaBarra").GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isDashing)
        {
            return;
        }
        if(_gameManager.isPlaying == false)
        {
            return;
        }

        inputHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump")) //GetButton tiene 3 formas (El Down que hace la accion cuando pulsa el boton, el Up que hace la acicon cuando sueltas el boton y el GetButton que hace manteniendo el boton)
        {
            if(_groundSensor.isGrounded || _groundSensor.canDoubleJump)
            {
                Jump();
            }
            
        }
        Movement();

        _animator.SetBool("IsJumping", !_groundSensor.isGrounded);

        if(Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }

        if(Input.GetButtonDown("Espadazo"))
        {
            _animator.SetTrigger("IsAttacking");
        }

        /*if(Input.GetButton("EspadazoF"))
        {
            AttackCharge();
        }

        if(Input.GetButtonUp("EspadazoF"))
        {
            ChargedAttack();
        }*/

        if(Input.GetButtonDown("Fireball") && _hasMana)

        {
            Shoot(_fireballCost);
        }

        if(Input.GetButtonDown("Submit") && _IsChestHere)
        {
            _chests.OpenChest();
        }
    }

    void FixedUpdate()
    {
        if(_isDashing)
        {
            return;
        }

        if(_isCharging)
        {
            return;
        }

        rigidBody.velocity = new Vector2(inputHorizontal * Velocity, rigidBody.velocity.y);
    }

    void Movement()
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

    void FootStepsSound()
    {
        if(_groundSensor.isGrounded && Input.GetAxisRaw("Horizontal") != 0 && !_alreadyPlaying)
        {
            _particlesTransform.SetParent(gameObject. transform);
            _particlesTransform.localPosition = _particlesPosition;
            _particlesTransform.rotation = transform.rotation;
            _audioSource.Play();
            _particleSystem.Play();
            _alreadyPlaying = true;
        }
        else if(!_groundSensor.isGrounded || Input.GetAxisRaw("Horizontal") == 0)
        {
            _particlesTransform.SetParent(null);
            _audioSource.Stop();
            _particleSystem.Stop();
            _alreadyPlaying = false;
        }
    }

    void Jump()
    {
        if(!_groundSensor.isGrounded)
        {
            _groundSensor.canDoubleJump = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            //_animator.SetBool("Double", true);
            //rigidBody.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
        }
        /*else
        {
            _animator.SetBool("IsJumping", true);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }*/


        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //hacer que salte
        _animator.SetBool("IsJumping", true);
    }

    IEnumerator Dash()
    {
        float gravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        _animator.SetTrigger("IsDashing");
        _audioSource.PlayOneShot(dashSFX);

        _isDashing = true;
        _canDash = false;
        rigidBody.AddForce(transform.right * _dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_dashDuration);
        rigidBody.gravityScale = gravity;
        _isDashing = false;

        yield return new WaitForSeconds(_dashCoolDown);
        _canDash = true;
    }

    void NormalAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);
         _audioSource.PlayOneShot(_atackAudio); 
         foreach(Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(_attackDamage);
        }   
    }

    /*void AttackCharge()
    {
        if(_chargedAttackDamage < _maxChargedAttackDamage)
        {
            _chargedAttackDamage += Time.deltaTime;
            Debug.Log(_chargedAttackDamage);
        }
        else
        {
            _chargedAttackDamage = _maxChargedAttackDamage;
        }
       

    }

    void ChargedAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);
        foreach(Collider2D enemy in enemies)
        {
            //Enemy enemyScript = enemy.GetComponent<Enemy>();
            //enemyScript.TakeDamage(_chargedAttackDamage);
        }

        _chargedAttackDamage = _baseChargedAttackDamage;
    }*/

    void Shoot(float cost)
    {
        Instantiate(firePrefab, fireSpawn.position, fireSpawn.rotation);
        _animator.SetTrigger("IsShooting");
        _audioSource.PlayOneShot(shootSFX);

        _currentMana -= cost;
        _manaBar.fillAmount = _currentMana;

        if(_currentMana <= 0)
        {
            _hasMana = false;
        }
        else if(_currentMana > 0)
        {
            _hasMana = true;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_hitBoxPosition.position, _attackRadius);
    }

   public void Death()
    {
        _animator.SetTrigger("IsDeath");
        _audioSource.PlayOneShot(deathSFX);
        _boxCollider.enabled = false;

        Destroy(_groundSensor.gameObject);
        inputHorizontal = 0;
        //rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = 0;
        
        //rigidBody.AddForce(Vector2.up * jumpForce / 2, ForceMode2D.Impulse);
        
        _gameManager.isPlaying = false;

        StartCoroutine(_soundManager.DeathBGM());
        //_soundManager.StartCoroutine("DeathBGM");

        //_soundManager.Invoke("DeathBGM", deathSFX.length);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 10)
        {
            _chests = collider.gameObject.GetComponent<Cofres>();
            _IsChestHere = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        _IsChestHere = false;
    }

    public void TakeDamage(float damage)
    {
        _healthBar.fillAmount = _currentHealth -= damage;
        _audioSource.PlayOneShot(_damage);

        if(_currentHealth <= 0)
        {
            Death();
        }
    }

    public void RestoreMana()
    {
        _currentMana += 0.35f;
        _manaBar.fillAmount = _currentMana;
        _hasMana = true;
    }

    public void RestoreHealth()
    {
        _currentHealth += 0.5f;
        _healthBar.fillAmount = _currentHealth;
    }

}
