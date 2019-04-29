using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
    {
        Animator anim;
        public float speed = 10;
        public float DASH_DISTANCE;
        Sprite toSp;

        Rigidbody2D rb2D;
        // Start is called before the first frame update
        void Start()
        {
            toSp = this.GetComponent<SpriteRenderer>().sprite;
            rb2D = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void FixedUpdate()
        {
            MoveCharacter();
            MouseRotation();
        }

        void MoveCharacter()
        {
            rb2D.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
        }

        void MouseRotation()
        {
            Vector3 mouse_pos = Input.mousePosition;
            mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);

            Vector2 dir = new Vector2(
                mouse_pos.x - transform.position.x,
                mouse_pos.y - transform.position.y
            );

            transform.up = dir;
        }
 }
