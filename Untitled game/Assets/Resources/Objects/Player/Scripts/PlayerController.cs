using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PlayerController
{  
    public void MoveCharacter(Rigidbody2D body, float speed)
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
    }

    public void MouseRotation(Transform transform)
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
