using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using DG.Tweening;

public class Skeleton : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 targetPoint;
    float distanceToTarget;

    RaycastHit hit;
    float coolDown;
    string name;

    Animator anim;
    GameObject target;

    bool start;

    public GameObject myArea;
    public float health;
    public float maxHealth;
    public float damage;
    public GameObject particle;
    public GameObject healthBar;
    public int level;
    public float healthBuff;
    public float damageBuff;

    public GameObject arrow;
    public GameObject bullet;
    public GameObject arrowArea;

    float lastHealth;
    GameObject lastInfo;

    NavMeshAgent agent;

    float distance;
    float Range;

    float time;

    bool dead;

    GameObject myBody;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindObjectOfType<Player>().gameObject;
        anim.SetInteger("WalkType", Random.Range(1, 3));
        Invoke("Starting", Random.Range(0f, 2f));
        firstPos = transform.position;

        time = 10;
        lastHealth = maxHealth;

        int rnd = Random.Range(1, transform.childCount - 2);
        transform.GetChild(rnd).gameObject.SetActive(true);

        for (int i = 1; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                name = transform.GetChild(i).name;
                myBody = transform.GetChild(i).gameObject;
                break;
            }
        }

        if (myArea && !myArea.GetComponent<ZombieManager>().totalZombie.Contains(gameObject))
        {
            myArea.GetComponent<ZombieManager>().totalZombie.Add(gameObject);
        }

        //RAGDOLL
        SetRagdoll(false);

        //BUFF
        maxHealth += maxHealth * (healthBuff * (level - 1));
        damage += damage * (damageBuff * (level - 1));
        health = maxHealth;
    }
    private void OnDestroy()
    {
        if (myArea && myArea.GetComponent<ZombieManager>().totalZombie.Contains(gameObject))
        {
            myArea.GetComponent<ZombieManager>().totalZombie.Remove(gameObject);
        }
        if (target && target.GetComponent<Player>().enemies.Contains(gameObject))
        {
            target.GetComponent<Player>().enemies.Remove(gameObject);
        }
        if (target && target.GetComponent<Player>().combatEnemies.Contains(gameObject))
        {
            target.GetComponent<Player>().combatEnemies.Remove(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        if (agent.velocity == Vector3.zero)
            transform.position = firstPos;
        else
            firstPos = transform.position;

        if (!agent.isOnNavMesh || myArea == null)
        {
            Destroy(gameObject);
        }

        if (start)
        {
            healthBar.transform.GetChild(0).localScale = new Vector3(health / maxHealth, 1, 1);
            healthBar.transform.GetChild(1).GetChild(1).GetComponent<TextMeshPro>().text = level.ToString();
            healthBar.transform.parent.LookAt(UIActor.Instance.playerCam.transform.position);

            if (lastHealth > health)
                GetDamageInfo();

            if (health >= maxHealth)
            {
                healthBar.SetActive(false);
            }
            else
            {
                healthBar.SetActive(true);
            }

            distance = Vector3.Distance(target.transform.position, transform.position);

            if (myArea.GetComponent<ZombieManager>().needZombie != 5)
            {
                if (distance < 50)
                {
                    myBody.SetActive(true);
                    healthBar.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    myBody.SetActive(false);
                    healthBar.transform.parent.gameObject.SetActive(false);
                }
            }

            if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }

            if (!target.GetComponent<Player>().inBase && myArea == target.GetComponent<Player>().area)
            {
                if (distance < 15f)
                {
                    agent.isStopped = true;
                    anim.SetBool("Walk", false);
                    transform.LookAt(target.transform.position);
                    agent.Stop();
                    if (coolDown <= 0)
                    {
                        arrow.SetActive(true);
                        anim.SetBool("Bow", true);
                        Invoke("Hit", 1.1f);
                        coolDown = 2.1f;
                    }
                }
                if (distance < 20 && distance >= 15)
                {
                    agent.Resume();
                    agent.SetDestination(target.transform.position);
                    agent.isStopped = false;
                    anim.SetBool("Walk", true);
                    anim.SetBool("Bow", false);
                }
                else
                {
                    time -= Time.deltaTime;

                    if (time <= 0)
                    {
                        if (Random.Range(0, 5) == 1)
                        {
                            agent.isStopped = false;
                            anim.SetBool("Walk", true);
                            agent.SetDestination(GetRandomPoint(transform, 20f));
                        }
                        else
                        {
                            anim.SetBool("Walk", false);
                        }
                        time = 5f;
                    }
                }
            }
            else
            {
                time -= Time.deltaTime;

                if (time <= 0)
                {
                    if (Random.Range(0, 5) == 1)
                    {
                        agent.isStopped = false;
                        anim.SetBool("Walk", true);
                        targetPoint = GetRandomPoint(transform, 10f);
                        distanceToTarget = Vector3.Distance(transform.position, targetPoint);
                        agent.SetDestination(targetPoint);
                    }
                    else
                    {
                        agent.isStopped = true;
                        anim.SetBool("Walk", false);
                    }
                    time = 5f;
                }
            }


            if (health <= 0)
            {
                Player player = GameObject.FindObjectOfType<Player>();
                player.enemies.Remove(gameObject);
                Dead();
            }

        }
    }

    public void Dead()
    {
        SetRagdoll(true);
        dead = true;
        healthBar.transform.parent.gameObject.SetActive(false);
        Invoke("DropHead", 1.25f);
        anim.SetBool("Bow", false);
    }

    public void GetDamageInfo()
    {
        float damage = lastHealth - health;
        GameObject info = Instantiate(UIActor.Instance.info, transform.position + (transform.up * 7), Quaternion.identity);
        info.SetActive(true);
        int child = Random.Range(0, info.transform.childCount);
        info.transform.GetChild(child).gameObject.SetActive(true);
        lastInfo = info;
        info.transform.LookAt(UIActor.Instance.mainCam.transform.position);
        info.transform.GetChild(child).GetComponent<TextMeshPro>().text = (int)damage + "";
        info.transform.GetChild(child).GetComponent<TextMeshPro>().color = Color.white;
        lastHealth = health;
    }

    public void DropHead()
    {
        GameObject newObj = Instantiate(Resources.Load<GameObject>("Heads"), transform.position + new Vector3(0, 2, 0), transform.rotation);
        newObj.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);

        if (target.GetComponent<Player>().x2Head)
        {
            GameObject newObj2 = Instantiate(Resources.Load<GameObject>("Heads"), transform.position + new Vector3(0, 2, 0), transform.rotation);
            newObj2.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);

            for (int i = 0; i < newObj2.transform.childCount; i++)
            {
                if (newObj2.transform.GetChild(i).name == name)
                {
                    newObj2.transform.GetChild(i).gameObject.SetActive(true);
                    newObj2.name = name;
                    break;
                }
            }
        }

        if (myArea.GetComponent<ZombieManager>().totalZombie.Contains(gameObject))
        {
            myArea.GetComponent<ZombieManager>().totalZombie.Remove(gameObject);
        }

        for (int i = 0; i < newObj.transform.childCount; i++)
        {
            if (newObj.transform.GetChild(i).name == name)
            {
                newObj.transform.GetChild(i).gameObject.SetActive(true);
                newObj.name = name;
                break;
            }
        }

        Destroy(gameObject);
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

        if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? Range : radius, out _point))
        {
            return _point;
        }

        return Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.transform.GetComponent<Bullet>();
        switch (other.tag)
        {
            case "Bullet":
                switch (bullet.name)
                {
                    case "Pistol":
                        health -= Random.Range((float)bullet.damage.x, (float)bullet.damage.y);
                        Destroy(other.gameObject);
                        break;
                    case "Shotgun":
                        health -= Random.Range((float)bullet.damage.x, (float)bullet.damage.y);
                        Destroy(other.gameObject);
                        break;
                    case "Crossbow":
                        health -= Random.Range((float)bullet.damage.x, (float)bullet.damage.y);
                        other.transform.GetComponent<Bullet>().speed = 0;
                        other.transform.SetParent(transform);
                        Destroy(other.gameObject);
                        break;
                    case "Minigun":
                        health -= Random.Range((float)bullet.damage.x, (float)bullet.damage.y);
                        break;
                    case "Rocket":
                        health -= Random.Range((float)bullet.damage.x, (float)bullet.damage.y);
                        GameObject rocket = Instantiate(UIActor.Instance.rocketExplosion, other.transform.position, other.transform.rotation);
                        rocket.SetActive(true);
                        Destroy(other.gameObject);
                        break;
                }
                GameObject newParticle = Instantiate(particle, other.transform.position, other.transform.rotation);
                newParticle.SetActive(true);
                break;
            case "Ground":
                if (other.GetComponent<ZombieManager>())
                {
                    myArea = other.gameObject;
                    if (!other.GetComponent<ZombieManager>().totalZombie.Contains(gameObject))
                        other.GetComponent<ZombieManager>().totalZombie.Add(gameObject);
                }
                break;
        }
    }

    public void Hit()
    {
        arrow.SetActive(false);
        GameObject newBullet = Instantiate(bullet, arrowArea.transform.position, transform.rotation);
        newBullet.transform.LookAt(target.transform.position + new Vector3(0, 2.5f, 0));
        newBullet.SetActive(true);
    }

    public void Starting()
    {
        start = true;
    }

    public void SetRagdoll(bool active)
    {
        GetComponent<Collider>().enabled = !active;
        GetComponent<Animator>().enabled = !active;
        GetComponent<NavMeshAgent>().enabled = !active;
        //GetComponent<Rigidbody>().isKinematic = active;

        Rigidbody[] rigidbodies = transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = !active;

            if (active)
                rigidbody.AddForce(-transform.forward * 3000f);
        }

        Collider[] colliders = transform.GetChild(0).GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = active;
        }
    }
}
