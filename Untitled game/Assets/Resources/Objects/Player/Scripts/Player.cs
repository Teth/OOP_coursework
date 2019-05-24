using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{    
    public int damage;
    public int health;
    HealthController healthController;
    PlayerController playerController;    
    public float speed;
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
            if(Time.timeScale != 0)
            playerController.MouseRotation(transform);
        }
        else
        {
            body.rotation = 0;
            body.velocity = Vector2.zero;
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScene");
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
