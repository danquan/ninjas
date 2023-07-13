using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 350;

    private int numCoin = 0; // store for compute score
    
    private bool isGrounded;
    private bool isJumping;
    private bool isAttacking;
    private bool isThrowing;
    private bool isDead;

    //to save player's animation
    private string currentAnim;

    // to save velocity of player
    private float horizontal; //, vertical;

    private Vector3 savePoint;

    // Start is called before the first frame update
    void Start()
    {
        SavePoint();
        OnInit();
    }

    public void OnInit()
    {
        isDead = false;
        isAttacking = false;
        isJumping = false; 
        isThrowing = false;

        transform.position = savePoint;
        ChangeAnim("idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        { 
            return;
        }

        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        // If ninja is on the floor
        if (isGrounded)
        {
            // Jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else if (isJumping && rb.velocity.y < 0)
            {
                isJumping = false;
            }
            
            //Attack
            if(Input.GetKeyDown(KeyCode.J))
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
        else if (Mathf.Abs(rb.velocity.x) >= 0.1f && rb.velocity.y < 0)
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
        else if(isGrounded) // For sure, ninja's not jumping
        {
            if(!isAttacking && !isThrowing)
                ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    // Implement for Attacking
    private void Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        ChangeAnim("attack");

        Invoke(nameof(ResetAttack), 0.3f);
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }
    private void Throw()
    {
        isThrowing = true;
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");

        Invoke("ResetThrow", 0.3f);
    }
    private void ResetThrow()
    {
        isThrowing = false;
    }
    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
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
            ChangeAnim("die");
            isDead = true;

            Invoke(nameof(OnInit), 0.5f);
        }
    }
    private void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            //Debug.Log(currentAnim + " change to " + animName);
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
            //Debug.Log("It's now " + currentAnim);
            //Debug.Log(anim.name);
        }
    }
    private bool CheckGrounded()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        
        return hit.collider != null;
    }

}
