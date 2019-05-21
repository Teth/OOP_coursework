using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeletonController : IEnemyController
{
    public void Attack(Rigidbody2D body, int rotation, Vector2 direction)
    {
        body.rotation += rotation;
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
    public void Attack(Rigidbody2D body, int rotation, Vector2 direction)
    {
        ProjectileFactory fac = new ProjectileFactory();
        GameObject proj = Object.Instantiate(fac.GetProjectile(EnemyType.Skeleton).GetProjectilePrefab(),body.position,new Quaternion());
        proj.GetComponent<Rigidbody2D>().velocity = direction * proj.GetComponent<Projectile>().speed;
        proj.GetComponent<Rigidbody2D>().rotation = body.rotation;
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