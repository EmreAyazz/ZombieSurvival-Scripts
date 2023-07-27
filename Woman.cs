using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Woman : MonoBehaviour
{
    public bool tasima, tasindi;
    public GameObject target;

    public NavMeshAgent agent;
    float Range;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tasima)
        {
            transform.parent.position = target.transform.position;
            transform.parent.rotation = target.transform.rotation;
        }
        if (tasindi)
        {
            agent.enabled = true;
            time += Time.deltaTime;
            GetComponent<Animator>().SetBool("Walk", true);
            agent?.SetDestination(target.transform.position);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.GetAreaFromName("AI Walkable")))
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

        if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? Range : radius, out _point))
        {
            return _point;
        }

        return Vector3.zero;
    }
}
