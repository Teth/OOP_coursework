using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bridge

public class Enemy : MonoBehaviour
{
    EnemyController enemyController;
    public float speed;
    Rigidbody2D body;
    GameObject player;
    public int damage;
    public int health;
    HealthController healthController;
    //
    public float agroRange;
    public float attackRange;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = new EnemyController(new FishController());
        healthController = new HealthController(health);
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    private bool IsWayToPlayerExists()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirectionToPlayer());
        Debug.Log("Ray hits " + hit.collider.tag);
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
            if (GetVectorToPlayer().magnitude < (int) agroRange)
            {
                if (IsWayToPlayerExists())
                {
                    if (GetVectorToPlayer().magnitude > (int) attackRange)
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
        Debug.Log("enemy was hit");        
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
}

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
        body.rotation = 0;
    }

    public void RotateToPlayer(Transform transform, Vector2 rotateDirection)
    {
        transform.up = rotateDirection;
    }
}