using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackRange = 0.1f;
    [SerializeField] private float moveSpeed = 250;
    [SerializeField] private bool isRight;
    [SerializeField] private GameObject attackArea;

    private IState currentState = null; // state of this enemy
    private Character target;

    // an attack event should be happen after at least 0.4s from the previous one
    private float attackTimer = 0f;

    public Character Target => target;

    private void Update()
    {
        // an attack event should be happen after at least 0.4s from the previous one
        attackTimer = Mathf.Min(attackTimer + Time.deltaTime, 0.4f);
        //Debug.Log("Add time HERE: " + attackTimer);

        //Debug.Log("Current state: " + currentState.ToString() + ", enemy is running " + rb.velocity);
        if (currentState != null)
        {
            currentState.OnExcute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeactiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
        Invoke(nameof(OnDespawn), 1.5f);
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }

    }

    internal void SetTarget(Character character)
    {
        this.target = character;

        if(target == null) 
        {
            ChangeState(new IdleState());
        }
        else if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else 
        {
            ChangeState(new PatroiState());    
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed * Time.fixedDeltaTime;
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
    public void Idle()
    {
        ChangeAnim("idle");
    }
    
    public void Attack()
    {
        if (attackTimer >= 0.4f)
        {
            //Debug.Log("attack time: " + attackTimer);
            ChangeAnim("attack");
            attackTimer = 0f;
            ActiveAttack();
            Invoke(nameof(DeactiveAttack), 0.3f);
        }
    }
    public void ActiveAttack()
    {
        //Debug.Log("enemy active attack here");
        attackArea.SetActive(true);
    }

    public void DeactiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        rb.transform.rotation = Quaternion.Euler(new Vector3(0, isRight ? 0 : 180, 0));
    }

    public bool IsTargetInRange()
    {
        return Vector2.Distance(Target.transform.position, rb.transform.position) <= attackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }
}
