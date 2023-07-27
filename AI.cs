using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT.Weapon;
using UnityEngine.AI;
using DG.Tweening;

public class AI : MonoBehaviour
{
    GameObject target;
    float distance;
    bool walking;
    Animator anim;
    public NavMeshAgent navAgent;
    Player player;
    float bulletCoolDown;
    int totalStackCount;

    GameObject lastEnemy;

    public GameObject stackZone;

    public int weaponCount;
    public float speed;
    public GameObject bulletPos;
    public GameObject weapons;
    public Weapon myWeapon;
    public bool compPanel;

    public bool maxStack;
    public int maxStackCount;
    public int collectingStackCount;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<Player>().gameObject;
        anim = GetComponent<Animator>();
        navAgent = transform.parent.GetComponent<NavMeshAgent>();
        player = target.GetComponent<Player>();

        myWeapon = WeaponManager.Instance.weapons[weaponCount - 1];
        weapons.transform.GetChild(weaponCount - 1).gameObject.SetActive(true);
        anim.SetInteger("WeaponType_int", weaponCount);
    }

    // Update is called once per frame
    void Update()
    {
        myWeapon = WeaponManager.Instance.weapons[weaponCount - 1];

        navAgent.speed = player.transform.parent.GetComponent<JoystickPlayerExample>().speed;

        transform.GetChild(1).position = Vector3.zero;

        if (compPanel)
        {
            transform.parent.Rotate(Vector3.up * Time.deltaTime * 90);
        }

        if (target)
            distance = Vector3.Distance(transform.position, target.transform.position);
        else
            target = player.gameObject;

        if (bulletCoolDown > 0)
            bulletCoolDown -= Time.deltaTime;

        if (stackZone.transform.childCount - 1 >= maxStackCount)
        {
            maxStack = true;
        }
        else
        {
            maxStack = false;
        }

        if (player.enemies.Count > 0)
        {
            if (player.enemies[0].GetComponent<Enemy>() && player.enemies[0].GetComponent<Enemy>().myArea == player.area || player.enemies[0].GetComponent<BossEnemy>() && player.enemies[0].GetComponent<BossEnemy>().myArea == player.area || player.enemies[0].GetComponent<Skeleton>() && player.enemies[0].GetComponent<Skeleton>().myArea == player.area)
            {
                float dist = Vector3.Distance(transform.position, player.enemies[0].transform.position);
                if (bulletCoolDown <= 0 && dist <= myWeapon.range)
                {
                    anim.SetBool("Shoot_b", true);

                    if (myWeapon.bullet)
                    {
                        GameObject newBullet = Instantiate(myWeapon.bullet, bulletPos.transform.position, transform.rotation);
                        newBullet.transform.LookAt(player.enemies[0].transform.position + new Vector3(0, 2.5f, 0));

                        if (newBullet.GetComponent<Bullet>())
                            newBullet.GetComponent<Bullet>().damage = myWeapon.damage;
                        else
                        {
                            for (int i = 0; i < newBullet.transform.childCount; i++)
                            {
                                newBullet.transform.GetChild(i).GetComponent<Bullet>().damage = myWeapon.damage;
                            }
                        }

                        newBullet.SetActive(true);

                        if (myWeapon.muzzleEffect)
                        {
                            GameObject newMuzzle = Instantiate(myWeapon.muzzleEffect, bulletPos.transform.position, transform.rotation);
                            newMuzzle.SetActive(true);
                        }
                    }
                    else
                    {
                        lastEnemy = player.enemies[0];
                        Invoke("MeleeDamage", 0.5f);
                    }

                    bulletCoolDown = myWeapon.coolDown;

                    if (myWeapon.audio)
                    {
                        GetComponent<AudioSource>().clip = myWeapon.audio;
                        GetComponent<AudioSource>().Play();
                    }
                }
                if (dist > myWeapon.range)
                {
                    anim.SetBool("Shoot_b", false);
                }
            }
        }
        else
        {
            if (!compPanel)
            {
                anim.SetBool("Shoot_b", false);
            }
        }

        if (distance > 30)
        {
            navAgent.nextPosition = GetRandomPoint(player.transform, 3f);
        }

        if (navAgent.isOnNavMesh)
        {
            if (myWeapon.bullet)
            {
                if (distance > 5f)
                {
                    navAgent.isStopped = false;
                    walking = true;
                }
                else
                {
                    navAgent.isStopped = true;
                    walking = false;
                }
            }
            else
            {
                navAgent.isStopped = false;
                if (player.enemies.Count > 0)
                {
                    target = player.enemies[0];
                }
                else
                {
                    target = player.gameObject;

                }
                if (player.enemies.Count <= 0 && distance <= 5f)
                {
                    walking = false;
                    navAgent.isStopped = true;
                }
                else
                {
                    walking = true;
                    navAgent.isStopped = false;
                }
            }
        }

        if (!compPanel)
        {
            navAgent.enabled = true;
            if (walking)
            {
                navAgent.Resume();
                navAgent.SetDestination(target.transform.position);
                anim.SetFloat("Speed_f", 0.51f);
            }
            else
            {
                navAgent.Stop();
                anim.SetFloat("Speed_f", 0f);
            }
        }
        else
        {
            navAgent.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Head":
                if (!maxStack)
                {
                    Collecting(other.name, 1);
                    Destroy(other.gameObject);
                }
                break;
        }
    }
    public void MeleeDamage()
    {
        if (lastEnemy)
        {
            float rnd = Random.Range(myWeapon.damage.x, myWeapon.damage.y);
            if(lastEnemy.GetComponent<Enemy>())
            lastEnemy.GetComponent<Enemy>().health -= rnd;
            if (lastEnemy.GetComponent<BossEnemy>())
                lastEnemy.GetComponent<BossEnemy>().health -= rnd;
        }
    }
    public void Collecting(string name, int total)
    {
        GameObject mainPos = stackZone.transform.GetChild(0).gameObject;

        totalStackCount = stackZone.transform.childCount - 1;

        GameObject newObj = Instantiate(mainPos, mainPos.transform.parent);
        for (int i = 0; i < newObj.transform.childCount; i++)
        {
            if (newObj.transform.GetChild(i).name == name)
                newObj.transform.GetChild(i).gameObject.SetActive(true);
        }
        newObj.transform.localPosition = new Vector3((totalStackCount % 2 == 0 ? 0.2f : -0.2f), mainPos.transform.localPosition.y + (0.5f * ((totalStackCount / 2))), -0.5f);
        newObj.transform.localRotation = mainPos.transform.localRotation;
    }
    public void DestroyLastStackObejct()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
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
