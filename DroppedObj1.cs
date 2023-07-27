using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedObj1 : MonoBehaviour
{
    public Vector3 toPos;

    public float time;
    float distance;
    GameObject player;
    GameObject ai;

    GameObject target;
    bool onWay;

    // Start is called before the first frame update
    void Start()
    {
        time = 1f;
        player = GameObject.FindObjectOfType<CarControl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (target)
            distance = Vector3.Distance(transform.position, target.transform.position);
        else
            distance = 25;

        target = player;

        if (time <= 0 && target && !onWay)
        {
            onWay = true;
        }

        if (onWay)
        {
            if (target)
            {
                toPos = target.transform.position;
                distance = Vector3.Distance(toPos, transform.position);
                transform.Translate(Vector3.up * Time.deltaTime * 50f, Space.World);
                transform.position = Vector3.MoveTowards(transform.position, toPos, Time.deltaTime * 150f);

                if (distance < 10f)
                {
                    GameObject.FindObjectOfType<Player>().myItems.money += GetComponent<Money>().money;
                    Destroy(gameObject);
                }
            }
        }
    }
}
