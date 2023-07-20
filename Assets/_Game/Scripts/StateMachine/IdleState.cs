using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        enemy.Idle();
        timer = 0;
        randomTime = Random.Range(1.5f, 3.5f);
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if(timer > randomTime)
        {
            enemy.ChangeState(new PatroiState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}

