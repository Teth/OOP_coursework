using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bridge

public class Enemy : MonoBehaviour
{
    bool isPlayerAppeared;
    EnemyController enemyController;
    public float speed;
    Rigidbody2D body;
    GameObject player;
    public int damage;
    public int health;
    HealthController healthController;
    Vector2 playerLastEnterance;
    //
    public float agroRange;
    public float attackRange;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerAppeared = false;
        enemyController = new EnemyController(new FishController());
        healthController = new HealthController(health);
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    private bool IsWayToPlayerExists()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirectionToPlayer());
        if (hit.collider.tag == "Player")
        {
            return true;
        }
        return false;
    }
    private Vector2 GetVectorToPlayer()
    {
        return new Vector2(player.transform.position.x - body.position.x, player.transform.position.y - body.position.y);
    }
    private Vector2 GetDirectionToPlayer()
    {
        Vector2 magnitude = GetVectorToPlayer();
        magnitude.Normalize();
        return magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthController.IsAlive)
        {
            var vectorToPlayer = GetVectorToPlayer();
            var distanceToPlayer = GetVectorToPlayer().magnitude;            
            if (distanceToPlayer < (int)agroRange && IsWayToPlayerExists())
            {
                isPlayerAppeared = true;
                playerLastEnterance = player.transform.position;
                Debug.DrawLine(body.position, playerLastEnterance, Color.red, 1);
                if (distanceToPlayer > (int)attackRange)
                {
                    enemyController.RotateToPlayer(transform, GetDirectionToPlayer());
                    enemyController.Move(body, speed, GetDirectionToPlayer());
                }
                //else if (GetVectorToPlayer().magnitude < 3)
                //{
                //    enemyController.Flee(body, speed, GetDirectionToPlayer());
                //}
                else
                {
                    enemyController.Attack(body, -5);
                }
            }
            else if (isPlayerAppeared)
            {
                enemyController.MoveToPosition(body, speed, playerLastEnterance);
            }
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Enemy is DEAD");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            healthController.ReceiveDamage(((Player)collision.collider.GetComponent("Player")).damage);
        }
    }
}

public interface IEnemyController
{
    void Attack(Rigidbody2D body, int rotation);
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
    public void Attack(Rigidbody2D body, int rotation)
    {
        enemyController.Attack(body, rotation);
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

//Concrete Abstraction
public class FishController : IEnemyController
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
        Debug.Log(destination);
        //Debug.Log((destination - body.position).magnitude);
        if ((destination - body.position).magnitude > 0.1)
        {
            Move(body, speed, (destination - body.position).normalized);
            //Debug.Log("PROIDEMTE");
        }
        else
        {
            Debug.Log("PLEASE STOP");
            body.velocity = Vector2.zero;            
        }
    }
}