using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyController _enemy;
    public void Enter(EnemyController enemy)
    {
        _enemy = enemy;
        _enemy.SetAnimation("isAttacking",true);
    }   

    public void Exit()
    {
        _enemy.SetAnimation("isAttacking", false);
    }
}
