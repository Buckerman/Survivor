using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IEnemyState
{
    private EnemyController _enemy;
    public void Enter(EnemyController enemy)
    {
        _enemy = enemy;
        _enemy.SetAnimation("isWalking",true);
    }

    public void Exit()
    {
        _enemy.SetAnimation("isWalking", false);

    }
}
