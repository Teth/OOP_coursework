using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform bar;

    [SerializeField]
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position + transform.up * 0.8f;
        }
        else
        {
            Debug.Log("Destroynig from bar");
            Destroy(gameObject);
        }
    }

    public void setSize(float sizeNorm)
    {
        float initialScale = bar.localScale.x;
        bar.localScale = new Vector3(sizeNorm, 1f);
    }

    public void setTarget(GameObject objTarget)
    {
        target = objTarget.transform;
    }
}
