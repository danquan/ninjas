using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroiState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(1.8f, 3.6f);
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if(enemy.Target != null)
        {
            enemy.ChangeDirection(enemy.Target.transform.position.x >= enemy.transform.position.x);

            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
        }

        if (timer < randomTime)
        {
            enemy.Moving();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
