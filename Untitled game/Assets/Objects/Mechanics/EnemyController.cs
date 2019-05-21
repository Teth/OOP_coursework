using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyController
{
    void Attack(Rigidbody2D body, int damage, Vector2 direction);
    void Move(Rigidbody2D body, float speed, Vector2 direction);
    void RotateToPlayer(Transform transform, Vector2 rotateDirection);
    void Flee(Rigidbody2D body, float speed, Vector2 direction);
    void MoveToPosition(Rigidbody2D body, float speed, Vector2 destination);
}

//Abstraction
public class EnemyController
{
    private IEnemyController enemyController;
    public EnemyController(IEnemyController concreteEnemyController)
    {
        enemyController = concreteEnemyController;
    }
    public void Attack(Rigidbody2D body, int damage, Vector2 direction)
    {
        enemyController.Attack(body, damage, direction);
    }
    public void Flee(Rigidbody2D body, float speed, Vector2 direction)
    {
        enemyController.Flee(body, speed, direction);
    }
    public void Move(Rigidbody2D body, float speed, Vector2 direction)
    {
        enemyController.Move(body, speed, direction);
    }
    public void RotateToPlayer(Transform transform, Vector2 rotateDirection)
    {
        enemyController.RotateToPlayer(transform, rotateDirection);
    }
    public void MoveToPosition(Rigidbody2D body, float speed, Vector2 destination)
    {
        enemyController.MoveToPosition(body, speed, destination);
    }
}