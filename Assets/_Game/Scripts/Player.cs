using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunai;
    [SerializeField] private Transform kunaiThrowPoint;
    [SerializeField] private GameObject attackArea;

    private int numCoin = 0; // store for compute scores
    private int jumpButton = 0;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;
    private bool isThrowing;

    // an attack event should be happen after at least 0.4s from the previous one
    private float attackTimer = 0f;

    // to save velocity of player
    private float horizontal; //, vertical;

    private Vector3 savePoint;

    // Start is called before the first frame update
    void Start()
    {
        SavePoint();
        OnInit();
    }
    public override void OnInit()
    {
        // for default setting
        base.OnInit();

        isAttacking = false;
        isJumping = false; 
        isThrowing = false;

        // default Coins
        UIManager.instance.SetCoin(0);

        transform.position = savePoint;
        ChangeAnim("idle");

        DeactiveAttack();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        Invoke(nameof(OnInit), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // an attack event should be happen after at least 0.4s from the previous one
        attackTimer = Mathf.Min(attackTimer + Time.deltaTime, 0.4f);

        // Update Coin
        UIManager.instance.SetCoin(numCoin);

        //Debug.Log("Current HP: " + hp);
        if(IsDead())
        { 
            return;
        }

        isGrounded = CheckGrounded();

        // Get Horizontal Movement
        /*if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            SetDirectionHorizontal(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            SetDirectionHorizontal(1);
        else
            SetDirectionHorizontal(0);*/

        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

            // If ninja is on the floor
        if (isGrounded)
        {
            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || jumpButton == 1)
            {
                Jump();
            }
            
            // Stop Jumping
            if (isJumping && rb.velocity.y <= 0)
            {
                StopJump();
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
            }

            //Throw
            if(Input.GetKeyDown(KeyCode.K))
            {
                Throw();
            }
        }
        // Falling down
        else if (/*Mathf.Abs(rb.velocity.x) >= 0.1f &&*/ rb.velocity.y < 0)
        {
            ChangeAnim("fall");
        }

        // moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            if (!isJumping && !isAttacking && !isThrowing) 
            {
                ChangeAnim("run");
            }

            if (!isAttacking && !isThrowing)
            {
                rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal < 0 ? 180 : 0, 0));
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        // idle
        else if(isGrounded && !isJumping) // For sure, ninja's not jumping
        {
            Debug.Log("HERE");
            if(!isAttacking && !isThrowing)
                ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

        ///// Reset Button Up
        jumpButton = 0;
    }
    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    // Implement for Attacking
    public void Attack()
    {
        rb.velocity = Vector2.zero;

        // an attack event should be happen after at least 0.4s from the previous one
        if (attackTimer >= 0.4f)
        {
            isAttacking = true;
            ChangeAnim("attack");
            ActiveAttack();
            attackTimer = 0f;
            Invoke(nameof(ResetAttack), 0.3f);
        }

    }
    private void ResetAttack()
    {
        isAttacking = false;
        DeactiveAttack();
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeactiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void Throw()
    {
        isThrowing = true;
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");
        Instantiate(kunai, kunaiThrowPoint.position, kunaiThrowPoint.rotation);

        Invoke("ResetThrow", 0.3f);
    }
    private void ResetThrow()
    {
        isThrowing = false;
    }

    public void JumpButton(int jumpButton)
    {
        this.jumpButton = jumpButton;
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    private void StopJump()
    {
        isJumping = false;
    }
    private bool CheckGrounded()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        
        return hit.collider != null;
    }

    public void SetDirectionHorizontal(float horizontal)
    {
        //Debug.Log("Direction: " + (horizontal < 0 ? "Left" : "Right"));
        this.horizontal = horizontal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            //Debug.Log("Coin " +  collision.gameObject.name);
            ++numCoin;
            Destroy(collision.gameObject);
        }
        if(collision.tag == "DeathZone")
        {
            //Debug.Log("Must Dead");
            OnDeath();
        }

    }

}
