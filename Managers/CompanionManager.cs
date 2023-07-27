using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT.Weapon;
using UnityEngine.AI;

public class CompanionManager : MonoBehaviour
{
    public static CompanionManager Instance;

    public List<Companions> companions;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < companions.Count; i++)
        {
            if (LevelManager.Instance.levelCount >= companions[i].level)
            {
                if (companions[i].area == null)
                {
                    GameObject companion = Instantiate(companions[i].companion, GetRandomPoint(UIActor.Instance.center.transform, 10f), companions[i].companion.transform.rotation);
                    Companion comp = companion.GetComponent<Companion>();
                    comp.go = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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
