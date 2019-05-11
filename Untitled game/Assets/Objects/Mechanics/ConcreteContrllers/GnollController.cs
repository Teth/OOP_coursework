using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGnollController : IEnemyController
{
    public void Attack(Rigidbody2D body, int rotation)
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
public class RangedGnollController : IEnemyController
{
    public void Attack(Rigidbody2D body, int rotation)
    {
        Debug.Log("ae bruh dat`s ranged attack");
        body.rotation += rotation;
        body.velocity = Vector2.zero;
    }

    public void Flee(Rigidbody2D body, float speed, Vector2 direction)
    {
        Debug.Log("ae bruh dat`s ranged flee");
        body.velocity = -1 * speed * direction;
        body.rotation = 0;
    }

    public void Move(Rigidbody2D body, float speed, Vector2 direction)
    {
        Debug.Log("ae bruh dat`s ranged move");
        body.velocity = speed * direction;
    }

    public void RotateToPlayer(Transform transform, Vector2 rotateDirection)
    {
        Debug.Log("ae bruh dat`s ranged rotation");
        transform.up = rotateDirection;
    }
    public void MoveToPosition(Rigidbody2D body, float speed, Vector2 destination)
    {
        Debug.Log("ae bruh dat`s ranged move to position");
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