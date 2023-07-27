using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedObj : MonoBehaviour
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
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().dead)
        {
            Destroy(gameObject);
        }

        if (!ai)
        {
            if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Companion")].level > 0)
                ai = GameObject.FindObjectOfType<AI>().gameObject;
        }

        time -= Time.deltaTime;

        if (target)
            distance = Vector3.Distance(transform.position, target.transform.position);
        else
            distance = 25;

        if (tag == "Head")
        {
            if (!player.GetComponent<Player>().exchange)
            {
                if (player.GetComponent<Player>().maxStack && !player.GetComponent<Player>().unlimitedStorage)
                {
                    if (ai)
                    {
                        if (!ai.GetComponent<AI>().maxStack)
                        {
                            target = ai;
                        }
                        else
                        {
                            target = null;
                        }
                    }
                    else
                    {
                        target = null;
                    }
                }
                else
                    target = player;
            }
        }
        else
        {
            target = player;
        }

        if (time <= 0 && target && !onWay)
        {
            if (tag == "Head")
            {
                if (distance <= 15)
                {
                    player.GetComponent<Player>().collectingStackCount++;
                    onWay = true;
                }
            }
            else
            {
                onWay = true;
            }
        }

        if (onWay)
        {
            if (target)
            {
                toPos = target.transform.position;
                distance = Vector3.Distance(toPos, transform.position);
                transform.Translate(Vector3.up * Time.deltaTime * 50f, Space.World);
                transform.position = Vector3.MoveTowards(transform.position, toPos, Time.deltaTime * 75f);
            }
        }
    }
}
