using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    public int damage;
    public int health;
    ProxyHealthController healthController;
    PlayerController playerController;    
    public float speed;
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        healthController = new ProxyHealthController(health);
        playerController = new PlayerController();
        body = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthController.healthController.IsAlive)
        {
            playerController.MoveCharacter(body, speed);
            playerController.MouseRotation(transform);
        }
        else
        {
            body.rotation = 0;
            body.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            healthController.ReceiveDamage(collision.collider.GetComponent<Enemy>().damage);
        }
        else if (collision.collider.tag == "Projectile")
        {
            healthController.ReceiveDamage(collision.collider.GetComponent<Projectile>().damage);
        }
    }
}
