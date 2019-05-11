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
        enemyController = new EnemyController(new MeleeRatController());
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

    public void SetParameters(float agroRange, float attackRange, EnemyController enemyController, int health, float speed)
    {
        this.agroRange = agroRange;
        this.attackRange = attackRange;
        this.enemyController = enemyController;
        this.health = health;
        this.speed = speed;
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