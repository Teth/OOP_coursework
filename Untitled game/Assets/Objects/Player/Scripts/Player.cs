using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    public int damage;
    public int health;
    HealthController healthController;
    PlayerController playerController;    
    public float speed = 10;
    Rigidbody2D body;
    
    // Start is called before the first frame update
    void Start()
    {
        healthController = new HealthController(health);
        playerController = new PlayerController();
        body = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthController.IsAlive)
        {
            playerController.MoveCharacter(body, speed);
            playerController.MouseRotation(transform);
        }
        else
        {
            body.rotation = 0;
            body.velocity = Vector2.zero;
            Debug.Log("\n\nPlayer is DEAD\n\n");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            healthController.ReceiveDamage(((Enemy) collision.collider.GetComponent("Enemy")).damage);
        }
    }
}
