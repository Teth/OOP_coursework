using UnityEngine;

public class cameracontroll : MonoBehaviour
{    
    public Transform target;

    public float smooth_speed = 0.125F;
    public Vector3 offset;
    public float mouse_offset_value;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mp = Input.mousePosition;
        Vector3 curp = Camera.main.ScreenToWorldPoint(mp);
        Vector3 camera_off = curp - target.position;
        if (camera_off.magnitude >= mouse_offset_value)
        {
            camera_off = camera_off.normalized * mouse_offset_value;
        }
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset + camera_off / 2, ref velocity, 0.1F);
    }
}
