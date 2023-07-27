using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string name;
    public float speed;
    private float time;

    public GameObject myObject;
    public Vector3 myPoint;
    private float distance;
    public Vector2 damage;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        time += Time.deltaTime;

        if (time >= 3)
        {
            Destroy(gameObject);
        }
    }
}
