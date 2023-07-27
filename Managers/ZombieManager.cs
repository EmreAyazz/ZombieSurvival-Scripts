using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ZombieManager : MonoBehaviour
{
    public List<GameObject> totalZombie;
    public List<GameObject> bossList;
    public int needZombie;

    public GameObject zombie;
    public float spawnerTime;
    public float spawnTime;

    GameObject spawner;
    TextMeshPro spawnerTimeText;

    string spawnTimeStr;
    RaycastHit hit;
    Player player;

    public Vector3 centrePoint;
    public Vector3 totalPoint;
    public GameObject cube;
    bool boostB;

    public float boostTime = 0;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        spawner = transform.GetChild(0).gameObject;
        spawnerTimeText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        if (LevelManager.Instance.levelCount != 1)
            spawnerTime = 90;
        else
            spawnerTime = 99999;

        spawnTime = spawnerTime;
        cube.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (needZombie == 1)
        {
            int level = 0;
            if (LevelManager.Instance.levelCount % 10 != 0)
            {
                level = LevelManager.Instance.levelCount - (10 * (LevelManager.Instance.levelCount / 10));
            }
            else
            {
                level = LevelManager.Instance.levelCount - (10 * ((LevelManager.Instance.levelCount / 10) - 1));
            }
            if (zombie != Resources.Load<GameObject>($"Boss {level}"))
            {
                zombie = Resources.Load<GameObject>($"Boss {level}");
                if (totalZombie.Count <= 0)
                {
                    GameObject newZombie = Instantiate(zombie, GetRandomPoint(transform, 10f), Quaternion.identity);
                    newZombie.transform.eulerAngles = new Vector3(0, 180, 0);
                    newZombie.transform.SetParent(LevelManager.Instance.lastLevel.transform);
                    newZombie.SetActive(true);
                    if (newZombie.GetComponent<BossEnemy>())
                    {
                        newZombie.GetComponent<BossEnemy>().myArea = gameObject;
                        newZombie.GetComponent<BossEnemy>().boss = true;
                    }
                    totalZombie.Add(newZombie);
                }
            }
        }
        else
        {
            RetryZombies(true);
        }

        spawnTimeStr = (((int)(spawnerTime - spawnTime) / 60).ToString().Length == 1 ? "0" + ((int)(spawnerTime - spawnTime) / 60).ToString() : ((int)(spawnerTime - spawnTime) / 60).ToString()) +
            ":" + 
            (((int)(spawnerTime - spawnTime) - (((int)(spawnerTime - spawnTime) / 60) * 60)).ToString().Length == 1 ? "0" + ((int)(spawnerTime - spawnTime) - (((int)(spawnerTime - spawnTime) / 60) * 60)).ToString() : ((int)(spawnerTime - spawnTime) - (((int)(spawnerTime - spawnTime) / 60) * 60)).ToString());

        if (spawnerTime < 1000)
            spawnerTimeText.text = spawnTimeStr;
        else
            spawnerTimeText.text = "--:--";

        if (totalZombie.Count <= 0)
        {
            spawnerTimeText.transform.parent.gameObject.SetActive(true);
            GetComponent<MeshRenderer>().material.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        }
        else
        {
            spawnerTimeText.transform.parent.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0f);
        }
        if (player.area == gameObject && !boostB)
        {
            if (boostTime >= 2 && boostTime < 3)
            {
                for (int i = 0; i < BoostManager.Instance.boost.Count; i++)
                {
                    if (!BoostManager.Instance.boost[i].okay)
                    {
                        GameObject boost = Instantiate(Resources.Load<GameObject>("Rewardeds"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                        BoostManager.Instance.boost[i].okay = true;
                        boost.GetComponent<BoostObject>().name = BoostManager.Instance.boost[i].boostName;
                        boost.transform.Find(BoostManager.Instance.boost[i].boostName).gameObject.SetActive(true);
                        boost.transform.eulerAngles = new Vector3(0, 180, 0);
                        boost.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                        boostB = true;
                        break;
                    }
                }
            }
            else
            {
                boostTime += Time.deltaTime;
            }
        }

        if (needZombie > totalZombie.Count)
        {
            if (totalZombie.Count <= 0)
            {
                if (spawnTime <= 0)
                {
                    GameObject health = Instantiate(Resources.Load<GameObject>("Health"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    health.transform.eulerAngles = new Vector3(-90, 0, 0);
                    health.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);

                    int random = Random.Range(0, 2);
                    if (random == 0 && needZombie != 1)
                    {
                        GameObject key = Instantiate(Resources.Load<GameObject>("Key"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                        key.transform.eulerAngles = new Vector3(0, 0, -40);
                        key.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                    }

                    if (player.maxStack && needZombie != 1 && needZombie != 5)
                    {
                        UIActor.Instance.trader.transform.position = Vector3.zero;
                        UIActor.Instance.trader.SetActive(true);
                    }
                }
                spawnTime += Time.deltaTime;
            }

            if (spawnTime >= spawnerTime)
            {
                RetryZombies(false);
                spawnTime = 0;
            }
        }
        else
        {
            spawnTime = 0;
            spawnerTimeText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void RetryZombies(bool active)
    {
        if (active)
        {
            if (bossList.Count == LevelManager.Instance.levelCount - 1)
                return;
        }

        cube.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = LevelManager.Instance.levelCount.ToString();

        bossList.Clear();
        if (LevelManager.Instance.levelCount > 1)
        {
            for (int i = 1; i < LevelManager.Instance.levelCount; i++)
            {
                int level = 0;
                if (i % 5 != 0)
                {
                    level = i - (5 * (i / 5));
                }
                else
                {
                    level = i - (5 * ((i / 5) - 1));
                }
                bossList.Add(Resources.Load<GameObject>($"Boss {level}"));
            }
        }
        for (int i = 0; i < totalZombie.Count; i++)
        {
            Destroy(totalZombie[i]);
        }
        totalZombie.Clear();

        for (int i = 0; i < needZombie; i++)
        {
            if (bossList.Count > 0)
            {
                int rnd = Random.Range(0, 10);

                if (rnd == 5)
                {
                    GameObject newZombie = Instantiate(bossList[Random.Range(0, bossList.Count)], GetRandomPoint(transform, 10f), Quaternion.identity);
                    newZombie.SetActive(true);
                    newZombie.transform.SetParent(LevelManager.Instance.lastLevel.transform);
                    if (newZombie.GetComponent<BossEnemy>())
                        newZombie.GetComponent<BossEnemy>().myArea = gameObject;
                    totalZombie.Add(newZombie);
                }
                else
                {
                    GameObject newZombie = Instantiate(Random.Range(0, 4) == 1 ? Resources.Load<GameObject>("Skeleton").gameObject : zombie, GetRandomPoint(transform, 10f), zombie.transform.rotation);
                    newZombie.SetActive(true);
                    newZombie.transform.SetParent(LevelManager.Instance.lastLevel.transform);
                    if (newZombie.GetComponent<Enemy>())
                    {
                        newZombie.GetComponent<Enemy>().myArea = gameObject;
                        newZombie.GetComponent<Enemy>().level = LevelManager.Instance.levelCount;
                    }
                    if (newZombie.GetComponent<Skeleton>())
                    {
                        newZombie.GetComponent<Skeleton>().myArea = gameObject;
                        newZombie.GetComponent<Skeleton>().level = LevelManager.Instance.levelCount;
                    }
                    if (newZombie.GetComponent<BossEnemy>())
                        newZombie.GetComponent<BossEnemy>().myArea = gameObject;
                    totalZombie.Add(newZombie);
                }
            }
            else
            {
                GameObject newZombie = Instantiate(Random.Range(0, 4) == 1 ? Resources.Load<GameObject>("Skeleton").gameObject : zombie, GetRandomPoint(transform, 10f), zombie.transform.rotation);
                newZombie.SetActive(true);
                newZombie.transform.SetParent(LevelManager.Instance.lastLevel.transform);
                if (newZombie.GetComponent<Enemy>())
                {
                    newZombie.GetComponent<Enemy>().myArea = gameObject;
                    newZombie.GetComponent<Enemy>().level = LevelManager.Instance.levelCount;
                }
                if (newZombie.GetComponent<Skeleton>())
                {
                    newZombie.GetComponent<Skeleton>().myArea = gameObject;
                    newZombie.GetComponent<Skeleton>().level = LevelManager.Instance.levelCount;
                }
                if (newZombie.GetComponent<BossEnemy>())
                    newZombie.GetComponent<BossEnemy>().myArea = gameObject;
                totalZombie.Add(newZombie);
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
