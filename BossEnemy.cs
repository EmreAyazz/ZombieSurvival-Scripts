using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TT.Weapon;
using TMPro;
using CrazyGames;

public class BossEnemy : MonoBehaviour
{
    RaycastHit hit;
    float coolDown;
    string name;

    public Animator anim;
    GameObject target;

    bool start;
    public bool boss;
    public float damage;

    public int bounty;
    public GameObject myArea;
    public GameObject mouthArea;
    public float health;
    public float maxHealth;
    public float maxCoolDown;
    public GameObject particle;
    public GameObject hitParticle;
    public GameObject healthBar;
    public GameObject manaBar;
    public GameObject arrow;

    public NavMeshAgent agent;

    float lastHealth;
    GameObject lastInfo;

    float distance;
    float Range;

    float time;
    public float skillCoolDown;
    public bool inSkill;

    public List<Skill> skills;
    public int level;

    GameObject myBody;

    public bool happyTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindObjectOfType<Player>().gameObject;
        anim.SetInteger("WalkType", Random.Range(1, 3));
        Invoke("Starting", Random.Range(0f, 2f));

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

        skills[0].damage += (LevelManager.Instance.levelCount - level) * 10;
        skills[1].damage = skills[0].damage * 1.5f;
        damage = skills[0].damage - 25;
        maxHealth += (LevelManager.Instance.levelCount - level) * 1500;
        bounty += (LevelManager.Instance.levelCount - level) * 1500;

        if (!boss)
            maxHealth *= 0.4f;

        health = maxHealth;
        lastHealth = maxHealth;
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
        if (!agent.isOnNavMesh)
        {
            Destroy(gameObject);
        }

        healthBar.transform.GetChild(0).localScale = new Vector3(health / maxHealth, 1, 1);
        manaBar.transform.GetChild(0).localScale = new Vector3(skillCoolDown / skills[0].coolDown, 1, 1);
        healthBar.transform.parent.LookAt(UIActor.Instance.playerCam.transform.position);
        if (boss)
            healthBar.transform.parent.GetChild(2).GetChild(0).GetComponent<TextMeshPro>().text = $"{bounty}";
        else
            healthBar.transform.parent.GetChild(2).GetChild(0).GetComponent<TextMeshPro>().text = $"{bounty / 10}";

        if (lastHealth > health)
            GetDamageInfo();

        if (start && !inSkill)
        {

            if (inSkill)
                agent.isStopped = true;
            else
                agent.isStopped = false;

            distance = Vector3.Distance(target.transform.position, transform.position);

            if (!boss)
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

            if (distance > 50)
            {
                if (health < maxHealth)
                {
                    health += 2;
                }
            }
            if (distance > 100)
            {
                if (health < maxHealth)
                {
                    health = maxHealth;
                }
            }

            if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }

            if (!target.GetComponent<Player>().inBase && !target.GetComponent<Player>().dead && !inSkill && myArea == target.GetComponent<Player>().area)
            {
                if (distance < 7.5f)
                {
                    if (coolDown <= 0)
                    {
                        anim.SetBool("Attack", true);
                        Invoke("Hit", 1f);
                        coolDown = maxCoolDown;
                    }
                }
                if (distance < 20)
                {
                    if (!happyTime && boss)
                    {
                        happyTime = true;
                        CrazyEvents.Instance.HappyTime();
                    }

                    agent.SetDestination(target.transform.position);
                    agent.isStopped = false;
                    anim.SetBool("Walk", true);

                    if (!anim.GetBool("Attack"))
                    {
                        skillCoolDown += Time.deltaTime;
                        if (skillCoolDown > skills[0].coolDown)
                            SkillAt();
                    }
                }
                else
                {
                    if(happyTime)
                        happyTime = false;

                    agent.isStopped = true;
                    anim.SetBool("Walk", false);
                }
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("Walk", false);
            }
        }

        if (health <= 0)
        {
            Player player = GameObject.FindObjectOfType<Player>();
            player.enemies.Remove(gameObject);
            Dead();
            Destroy(gameObject);
        }
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

    public void SkillAt()
    {
        int rnd = Random.Range(0, skills.Count);
        switch (skills[rnd].skill.ToString())
        {
            case "Kaya":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Kaya(this));
                skillCoolDown = 0;
                break;
            case "YereVurma":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.YereVurma(this));
                skillCoolDown = 0;
                break;
            case "Tukuruk":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Tukuruk(this));
                skillCoolDown = 0;
                break;
            case "Kusma":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Kusma(this));
                skillCoolDown = 0;
                break;
            case "Chargelama":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Chargelama(this));
                skillCoolDown = 0;
                break;
            case "Ziplama":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Ziplama(this));
                skillCoolDown = 0;
                break;
            case "Fuze":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Fuze(this));
                skillCoolDown = 0;
                break;
            case "YukariFuze":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.YukariFuze(this));
                skillCoolDown = 0;
                break;
            case "Arrow":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Arrow(this));
                skillCoolDown = 0;
                break;
            case "AreaArrow":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.AreaArrow(this));
                skillCoolDown = 0;
                break;
            case "GurbuzFirlatma":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.GurbuzFirlatma(this));
                skillCoolDown = 0;
                break;
            case "GurbuzDonme":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.GurbuzDonme(this));
                skillCoolDown = 0;
                break;
            case "TekliTestere":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.TekliTestere(this));
                skillCoolDown = 0;
                break;
            case "Testere":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Testere(this));
                skillCoolDown = 0;
                break;
            case "FireBall":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.FireBall(this));
                skillCoolDown = 0;
                break;
            case "FlameThower":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.FlameThower(this));
                skillCoolDown = 0;
                break;
            case "Simsek":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Simsek(this));
                skillCoolDown = 0;
                break;
            case "ElektrikAlan":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.ElektrikAlan(this));
                skillCoolDown = 0;
                break;
            case "Alev":
                SkillManager.Instance.StartCoroutine(SkillManager.Instance.Alev(this));
                skillCoolDown = 0;
                break;
        }
    }


    public void Dead()
    {
        GameObject newObj = Instantiate(Resources.Load<GameObject>("Heads"), transform.position + new Vector3(0, 2, 0), transform.rotation);
        newObj.transform.localScale = newObj.transform.localScale * 2;
        newObj.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);

        if (boss)
        {
            LevelManager.Instance.StartCoroutine(LevelManager.Instance.BossPanel(level));

            if (LevelManager.Instance.levelCount <= 1)
            {
                Fence[] fences = GameObject.FindObjectsOfType<Fence>();
                foreach (Fence fence in fences)
                {
                    if (!fence.openingObject)
                    {
                        TutorialManager.Instance.queue.Add(fence.gameObject);
                    }
                }
            }
        }

        if (target.GetComponent<Player>().x2Head)
        {
            GameObject newObj2 = Instantiate(Resources.Load<GameObject>("Heads"), transform.position + new Vector3(0, 2, 0), transform.rotation);
            newObj2.transform.localScale = newObj.transform.localScale * 2;
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

        if (boss)
        {
            float moneyValue = bounty / 50f;
            for (int i = 0; i < bounty / moneyValue; i++)
            {
                GameObject money = Instantiate(Resources.Load<GameObject>("Money"), transform.position + new Vector3(0, 1, 0), transform.rotation);
                money.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                money.GetComponent<Money>().money = moneyValue;
            }
        }
        else
        {
            float moneyValue = bounty / 500f;
            for (int i = 0; i < bounty / moneyValue / 10f; i++)
            {
                GameObject money = Instantiate(Resources.Load<GameObject>("Money"), transform.position + new Vector3(0, 1, 0), transform.rotation);
                money.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                money.GetComponent<Money>().money = moneyValue;
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

        if (boss)
            LevelManager.Instance.TruckComing();
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
        if (distance <= 8)
        {
            if (boss)
                target.GetComponent<Player>().health -= (damage * ((100 - target.GetComponent<Player>().armor) / 100f));
            else
                target.GetComponent<Player>().health -= ((damage * 0.1f) * ((100 - target.GetComponent<Player>().armor) / 100f));
        }
        anim.SetBool("Attack", false);
    }

    public void Starting()
    {
        start = true;
        agent.isStopped = false;
        anim.SetBool("Walk", true);
        agent.SetDestination(GetRandomPoint(transform, 20f));
    }
}
