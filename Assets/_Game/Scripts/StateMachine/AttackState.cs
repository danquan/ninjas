using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeDirection(enemy.Target.transform.position.x >= enemy.transform.position.x);

        enemy.StopMoving();
        enemy.Attack();

        timer = 0;
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;


        if(!enemy.IsTargetInRange())
        {
            enemy.ChangeState(new PatroiState());
        }
        else if (enemy.IsTargetInRange()) 
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        enemy.DeactiveAttack();
    }
}
