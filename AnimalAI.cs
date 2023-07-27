using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AnimalAI : MonoBehaviour
{
    GameObject target;
    float distance;
    bool walking;
    Animator anim;
    public NavMeshAgent navAgent;
    Player player;

    [HideInInspector]
    public bool petPanel;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<Player>().gameObject;
        navAgent = GetComponent<NavMeshAgent>();
        player = target.GetComponent<Player>();
        anim = GetComponent<Animator>();

        player.stackBoost = 15;
    }

    // Update is called once per frame
    void Update()
    {
        navAgent.speed = player.transform.parent.GetComponent<JoystickPlayerExample>().speed;

        target = player.gameObject;
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (petPanel)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 90);
        }
        if (!petPanel)
            transform.DOLookAt(target.transform.position, 0.5f, AxisConstraint.Y);


        if (distance > 30)
        {
            navAgent.nextPosition = GetRandomPoint(player.transform, 3f);
        }
        if (distance > 5f)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        if (!petPanel)
        {
            navAgent.enabled = true;
            if (walking)
            {
                navAgent.Resume();
                navAgent.SetDestination(target.transform.position + (target.transform.right * 2));
                anim.SetFloat("Speed_f", 0.51f);
            }
            else
            {
                navAgent.Stop();
                anim.SetFloat("Speed_f", 0);
            }
        }
        else
        {
            navAgent.enabled = false;
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
