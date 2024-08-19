using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IEnemyState
{
    private Enemy _enemy;
    public void Enter(Enemy enemy)
    {
        _enemy = enemy;
        _enemy.SetAnimation("isWalking",true);
    }

    public void Exit()
    {
        _enemy.SetAnimation("isWalking", false);

    }
}
