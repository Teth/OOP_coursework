using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeletonController : IEnemyController
{
    public void Attack(Rigidbody2D body, int damage, Vector2 direction)
    {
        body.rotation += damage;
        body.velocity = Vector2.zero;
    }

    public void Flee(Rigidbody2D body, float speed, Vector2 direction)
    {
        body.velocity = -1 * speed * direction;
        body.rotation = 0;
    }

    public void Move(Rigidbody2D body, float speed, Vector2 direction)
    {
        body.velocity = speed * direction;
    }

    public void RotateToPlayer(Transform transform, Vector2 rotateDirection)
    {
        transform.up = rotateDirection;
    }
    public void MoveToPosition(Rigidbody2D body, float speed, Vector2 destination)
    {
        if ((destination - body.position).magnitude > 0.1)
        {
            Move(body, speed, (destination - body.position).normalized);
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
}
public class RangedSkeletonController : IEnemyController
{
    ProjectileFactory projectileFactory;

    public RangedSkeletonController()
    {
        projectileFactory = new ProjectileFactory();
    }

    public void Attack(Rigidbody2D body, int damage, Vector2 direction)
    {
        Debug.Log("Ranged skeletom attacks!");
        GameObject enemyObject = projectileFactory.GetProjectile(EnemyType.Skeleton).GetProjectilePrefab();
        GameObject gameObject = Object.Instantiate(enemyObject, body.transform);
        gameObject.GetComponent<Rigidbody2D>().velocity = direction.normalized * enemyObject.GetComponent<Projectile>().speed;
        WaitTimeForAttack();
    }

    public void Flee(Rigidbody2D body, float speed, Vector2 direction)
    {
        body.velocity = -1 * speed * direction;
        body.rotation = 0;
    }

    public void Move(Rigidbody2D body, float speed, Vector2 direction)
    {
        body.velocity = speed * direction;
    }

    public void RotateToPlayer(Transform transform, Vector2 rotateDirection)
    {
        transform.up = rotateDirection;
    }
    public void MoveToPosition(Rigidbody2D body, float speed, Vector2 destination)
    {
        if ((destination - body.position).magnitude > 0.1)
        {
            Move(body, speed, (destination - body.position).normalized);
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
    IEnumerator WaitTimeForAttack()
    {
        for (; ; )
        {            
            yield return new WaitForSeconds(3f);
        }
    }
}