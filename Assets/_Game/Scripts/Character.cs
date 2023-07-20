using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected HealtBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;
    [SerializeField] protected float defaultHP = 100f;
    private string currentAnim;

    protected float hp;

    public bool outOfHp => hp <= 0;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = defaultHP;
        //Debug.Log("HP now: " + hp);
        healthBar.OnInit(defaultHP);
    }

    public virtual void OnDespawn()
    {
    }

    protected virtual void OnDeath() 
    {
        ChangeAnim("die");
    }

    public void OnHit(float damage)
    {
        if(!outOfHp)
        {
            hp -= damage;

            // CombatText
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
            
            if(outOfHp)
            {
                hp = 0f;
                OnDeath();
            }

            healthBar.SetNewHP(hp);
        }
    }

    public bool IsDead()
    {
        return outOfHp;
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
}
