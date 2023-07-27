using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trader : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        target = player.gameObject;
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 30)
        {
            navAgent.nextPosition = GetRandomPoint(player.transform, 25f);
        }
        if (distance > 5f)
        {
            walking = true;
        }
        else
        {
            if (!LevelManager.Instance.bonusLevel)
            {
                UIActor.Instance.traderPanel.SetActive(true);
                Time.timeScale = 0;
            }
            walking = false;
        }

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
