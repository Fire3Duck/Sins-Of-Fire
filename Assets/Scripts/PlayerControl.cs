using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

private Rigidbody2D rigidBody;
private Animator _animator;
private GroundSensor _groundSensor;
private SpriteRenderer _spriteRender;
private BoxCollider2D _boxCollider;

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

public float inputHorizontal;

public float jumpForce = 10;

public float Velocity = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        _chargedAttackDamage = _baseChargedAttackDamage;
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isDashing)
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

        /*if(Input.GetButtonDown("Espadazo"))
        {
            NormalAttack();
        }*/

        if(Input.GetButton("EspadazoF"))
        {
            AttackCharge();
        }

        if(Input.GetButtonUp("EspadazoF"))
        {
            ChargedAttack();
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

    void Movement ()
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
        foreach(Collider2D enemy in enemies)
        {
            //Enemy enemyScript = enemy.GetComponent<Enemy>();
            //enemyScript.TakeDamage(_attackDamage);
        }   
    }

    void AttackCharge()
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
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_hitBoxPosition.position, _attackRadius);
    }
}
