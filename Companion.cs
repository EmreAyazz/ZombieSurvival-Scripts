using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TT.Weapon;

public class Companion : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public bool go;

    [HideInInspector]
    public Fence fence;

    [HideInInspector]
    public Animator anim;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public Companions comps;

    float time;
    Vector3 AITargetPos;

    bool completed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (transform.parent)
        {
            fence = transform.parent.parent.GetComponent<Fence>();
            fence.companion = gameObject;
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fence)
        {
            if (!go && fence.opened)
            {
                agent.SetDestination(fence.area.transform.position);
                anim.SetFloat("Speed_f", 0.51f);
                transform.parent.GetChild(1).gameObject.SetActive(false);

                fence.transform.GetChild(4).gameObject.SetActive(false);

                for (int i = 0; i < fence.money / 10; i++)
                {
                    GameObject newObj = Instantiate(Resources.Load<GameObject>("Money"), transform.position + new Vector3(0, 1, 0), transform.rotation);
                    newObj.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                    newObj.GetComponent<Money>().money = 10;
                }

                go = true;
            }
            if (go)
            {
                float distance = Vector3.Distance(transform.position, fence.area.transform.position);
                if (distance <= 1)
                {
                    anim.SetFloat("Speed_f", 0);

                    if (LevelManager.Instance.levelCount <= 1)
                        TutorialManager.Instance.queue.Add(fence.area.gameObject);

                    gameObject.SetActive(false);

                    if (fence.openingObject)
                    {
                        fence.openingObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (go)
            {
                if (target)
                {
                    anim.SetFloat("Speed_f", 0.51f);
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= 1)
                    {
                        for (int i = 0; i < comps.openingObjects.Count; i++)
                        {
                            comps.openingObjects[i].SetActive(true);
                        }
                        for (int i = 0; i < comps.closingObjects.Count; i++)
                        {
                            comps.closingObjects[i].SetActive(false);
                        }
                        Destroy(gameObject);
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                    if (time <= 0)
                    {
                        AITargetPos = GetRandomPoint(UIActor.Instance.center.transform, 10f);
                        agent.SetDestination(AITargetPos);
                        anim.SetFloat("Speed_f", 0.26f);
                        time = 10f;
                        agent.speed = 3;
                    }

                    float distance = Vector3.Distance(transform.position, AITargetPos);
                    if (distance <= 1)
                    {
                        anim.SetFloat("Speed_f", 0f);
                    }
                    else
                    {
                        anim.SetFloat("Speed_f", 0.26f);
                    }
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    public Vector3 GetRandomPoint(Transform point = null, float radius = 0)
    {
        Vector3 _point;

        if (RandomPoint(point == null ? transform.position : point.position, radius, out _point))
        {
            return _point;
        }

        return Vector3.zero;
    }
}
