using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using TT.Weapon;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    RaycastHit hit;
    float cooldown;
    float reqCoolDown;
    float healCoolDown;
    int totalStackCount;
    public bool dead;
    string hint;
    public float skillCooldown;
    public int[] skills = new int[3];

    Animator anim;

    public GameObject info;
    public GameObject cam;
    public GameObject stackZone;
    public GameObject slashEffect;

    public GameObject myHand;

    public ReqBoolList reqBoolList;
    public ReqNameCountList reqNameCountList;

    public MyItemCount myItems;

    public List<GameObject> enemies;
    public List<GameObject> combatEnemies;
    float bulletCoolDown;
    public GameObject bulletPos;
    public GameObject weapons;

    public GameObject cooldownBar;
    public float myBulletCoolDown;

    public float maxHealth;
    public float health;
    public float lastHealth;
    GameObject lastInfo;
    public GameObject healthBar;
    public float earnMoneyValue;

    public bool inBase;

    public float timeBar;
    float timeBarMax;
    public bool panel;
    public bool wpPanel;
    public bool maxStack;
    public int maxStackCount;
    public int collectingStackCount;

    float hasarCoolDown;

    public int armor;

    public GameObject inSkillArea;

    public Weapon myWeapon;
    public GameObject lastEnemy;

    public bool x2Damage, unlimitedStorage, x2Head;
    GameObject lastHead;
    int totalExchangeHead;
    float deadTimer;

    public GameObject portal;

    public float stackBoost;

    public bool exchange;

    GameObject companion;
    public GameObject area;

    [System.Serializable]
    public class MyItemCount
    {
        public float money;
        public float key;
        public int head;
    }

    [System.Serializable]
    public class ReqBoolList
    {
        public bool money;
        public bool head;
    }
    [System.Serializable]
    public class ReqNameCountList
    {
        public int money;
        public int head;
    }
    private void Awake()
    {
        if (PlayerPrefs.HasKey("UpgradeLevel 0"))
            Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("WeaponType_int", UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level - ((UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level - 1) / 10) * 10);

        myWeapon = WeaponManager.Instance.weapons[UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level - 1];
        weapons.transform.GetChild(UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level - 1).gameObject.SetActive(true);
        health = maxHealth;
        lastHealth = health;

        SetRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            UIActor.Instance.hintPanel.text = "HINT\n" + hint;
            UIActor.Instance.inGamePanel.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.deadPanel.gameObject.SetActive(true);

            UIActor.Instance.deadPanelTimerText.text = $"{(int)deadTimer}";
            if (deadTimer > 0)
            {
                deadTimer -= Time.deltaTime;
                UIActor.Instance.reviveButton.interactable = true;
            }
            else
            {
                UIActor.Instance.reviveButton.interactable = false;
                Retry();
            }

            UIActor.Instance.deadPanelTimer.fillAmount = deadTimer / 10f;
            return;
        }
        else
        {
            UIActor.Instance.inGamePanel.transform.parent.gameObject.SetActive(true);
            UIActor.Instance.deadPanel.gameObject.SetActive(false);
        }

        if (lastHealth > health)
            GetDamageInfo();
        if (health < (maxHealth * 0.25f))
            CanYanipSonme(UIActor.Instance.healthbar);
        else
            UIActor.Instance.healthbar.color = new Color(UIActor.Instance.healthbar.color.r, UIActor.Instance.healthbar.color.g, UIActor.Instance.healthbar.color.b, 1);
        if (maxStack)
            StackYanipSonme(UIActor.Instance.maxStackBar);
        else
            UIActor.Instance.maxStackBar.color = new Color(UIActor.Instance.maxStackBar.color.r, UIActor.Instance.maxStackBar.color.g, UIActor.Instance.maxStackBar.color.b, 1);

        if (skillCooldown > 0)
            skillCooldown -= Time.deltaTime;

        if (myItems.key >= 3)
        {
            if (portal)
                Destroy(portal);

            portal = Instantiate(UIActor.Instance.portal, GetRandomPoint(transform, 6f), Quaternion.identity);
            portal.transform.position = new Vector3(portal.transform.position.x, 2f, portal.transform.position.z);

            myItems.key = 0;
        }
        if (portal)
        {
            float distance = Vector3.Distance(portal.transform.position, transform.position);

            if (distance <= 3f)
            {
                GameObject bonus = Instantiate(UIActor.Instance.bonuslevel, UIActor.Instance.bonuslevel.transform.position, UIActor.Instance.bonuslevel.transform.rotation);
                bonus.SetActive(true);
                LevelManager.Instance.bonusLevel = bonus;
                StartCoroutine(LevelManager.Instance.LevelGecis());
                UIActor.Instance.joystick.SetActive(false);
                UIActor.Instance.healthbar.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.cooldownbar.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.swipeScreen.SetActive(true);
                UIActor.Instance.baseMap.SetActive(false);
                LevelManager.Instance.lastLevel.SetActive(false);
                transform.parent.position = new Vector3(9, 0, -20);
                LevelManager.Instance.start = false;
                Destroy(portal);
            }
        }

        UIActor.Instance.cooldownbar.fillAmount = bulletCoolDown / myWeapon.coolDown;
        UIActor.Instance.healthbar.fillAmount = health / maxHealth;
        UIActor.Instance.timeBar.fillAmount = timeBar / timeBarMax;
        UIActor.Instance.maxStackBar.fillAmount = ((float)stackZone.transform.childCount - 1) / ((float)maxStackCount);
        UIActor.Instance.moneyText.text = (int)myItems.money + "";

        if (wpPanel)
        {
            transform.parent.Rotate(Vector3.up * Time.deltaTime * 90);
        }
        if (panel)
            UIActor.Instance.inGamePanel.SetActive(false);
        else
        {
            UIActor.Instance.inGamePanel.SetActive(true);
        }

        if (enemies.Count > 0)
        {
            if (enemies[0].GetComponent<Enemy>() && enemies[0].GetComponent<Enemy>().myArea == area || enemies[0].GetComponent<BossEnemy>() && enemies[0].GetComponent<BossEnemy>().myArea == area || enemies[0].GetComponent<Skeleton>() && enemies[0].GetComponent<Skeleton>().myArea == area)
            {
                transform.DOLookAt(enemies[0].transform.position, 0.5f, AxisConstraint.Y);
                float dist = Vector3.Distance(transform.position, enemies[0].transform.position);
                anim.SetBool("Idle_b", true);

                if (bulletCoolDown <= 0 && dist <= myWeapon.range)
                {
                    anim.SetBool("Shoot_b", true);

                    if (myWeapon.bullet)
                    {
                        GameObject newBullet = Instantiate(myWeapon.bullet, bulletPos.transform.position, transform.rotation);
                        newBullet.transform.LookAt(enemies[0].transform.position + new Vector3(0, 2.5f, 0));

                        if (newBullet.GetComponent<Bullet>())
                            newBullet.GetComponent<Bullet>().damage = (x2Damage == false ? myWeapon.damage : myWeapon.damage * 2);
                        else
                        {
                            for (int i = 0; i < newBullet.transform.childCount; i++)
                            {
                                newBullet.transform.GetChild(i).GetComponent<Bullet>().damage = (x2Damage == false ? myWeapon.damage : myWeapon.damage * 2);
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
                        lastEnemy = enemies[0];
                        slashEffect.GetComponent<ParticleSystem>().Play();
                        Invoke("MeleeDamage", 0.5f);
                    }

                    bulletCoolDown = myWeapon.coolDown;
                    EnemiesKontrol();

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

                if (skillCooldown <= 0)
                {
                    if (skills[0] != 10 || skills[1] != 10 || skills[2] != 10)
                    {
                        int skill = 10;
                        while (skill == 10)
                        {
                            skill = skills[Random.Range(0, 3)];
                        }
                        if (skill != 10)
                        {
                            GameObject newSkill = Instantiate(SkillManager.Instance.skills[skill], enemies[0].transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
                            newSkill.GetComponent<PlayerSkill>().damage = PlayerSkillManager.Instance.skills[skill].damage;
                            newSkill.SetActive(true);
                            skillCooldown = 5f;
                        }
                    }
                }

            }
            else
            {
                if (transform.localRotation != Quaternion.identity)
                    transform.DOLocalRotate(Vector3.zero, 0.5f);
                anim.SetBool("Shoot_b", false);
            }
        }
        else
        {
            if (transform.localRotation != Quaternion.identity)
                transform.DOLocalRotate(Vector3.zero, 0.5f);
            anim.SetBool("Shoot_b", false);
        }

        if (transform.localPosition != Vector3.zero)
            transform.localPosition = Vector3.zero;

        if (bulletCoolDown > 0)
            bulletCoolDown -= Time.deltaTime;

        if (!unlimitedStorage)
        {
            if (stackZone.transform.childCount - 1 >= maxStackCount + (int)(maxStackCount * (stackBoost / 100)))
            {
                maxStack = true;
                UIActor.Instance.maxStackText.gameObject.SetActive(true);
                UIActor.Instance.headsText.gameObject.SetActive(false);
            }
            else
            {
                maxStack = false;
                UIActor.Instance.headsText.gameObject.SetActive(true);
                UIActor.Instance.maxStackText.gameObject.SetActive(false);
                UIActor.Instance.headsText.text = $"{stackZone.transform.childCount - 1}/{maxStackCount + (int)(maxStackCount * (stackBoost / 100))}";
            }
        }
        else
        {
            UIActor.Instance.headsText.text = $"{stackZone.transform.childCount - 1}/{maxStackCount + (int)(maxStackCount * (stackBoost / 100))}";
            UIActor.Instance.headsText.gameObject.SetActive(true);
            UIActor.Instance.maxStackText.gameObject.SetActive(false);
        }
        if (inBase)
        {
            Save();
        }

        if (health <= 0)
        {
            health = 0;
            Dead();
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) && Vector3.Distance(randomPoint, transform.position) > 3f)
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
    public void MeleeDamage()
    {
        for (int i = 0; i < combatEnemies.Count; i++)
        {
            float rnd = Random.Range(myWeapon.damage.x, myWeapon.damage.y);

            if (x2Damage)
                rnd *= 2;

            if (combatEnemies[i].GetComponent<Enemy>())
                combatEnemies[i].GetComponent<Enemy>().health -= rnd;
            if (combatEnemies[i].GetComponent<BossEnemy>())
                combatEnemies[i].GetComponent<BossEnemy>().health -= rnd;
            if (combatEnemies[i].GetComponent<Skeleton>())
                combatEnemies[i].GetComponent<Skeleton>().health -= rnd;
            EnemiesKontrol();

            if (combatEnemies[i].GetComponent<Enemy>())
            {
                GameObject newParticle = Instantiate(combatEnemies[i].GetComponent<Enemy>().particle, combatEnemies[i].transform.position, combatEnemies[i].transform.rotation);
                newParticle.SetActive(true);
            }

            if (combatEnemies[i].GetComponent<BossEnemy>())
            {
                GameObject newParticle = Instantiate(combatEnemies[i].GetComponent<BossEnemy>().particle, combatEnemies[i].transform.position, combatEnemies[i].transform.rotation);
                newParticle.SetActive(true);
            }

            if (combatEnemies[i].GetComponent<Skeleton>())
            {
                GameObject newParticle = Instantiate(combatEnemies[i].GetComponent<Skeleton>().particle, combatEnemies[i].transform.position, combatEnemies[i].transform.rotation);
                newParticle.SetActive(true);
            }
        }
    }

    public void EnemiesKontrol()
    {
        if (enemies.Count > 1)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (j + 1 <= enemies.Count - 1)
                    {
                        float dist = Vector3.Distance(transform.position, enemies[j].transform.position);
                        float dist2 = Vector3.Distance(transform.position, enemies[j + 1].transform.position);
                        if (dist2 < dist)
                        {
                            GameObject enemy = enemies[j];
                            enemies[j] = enemies[j + 1];
                            enemies[j + 1] = enemy;
                        }
                    }
                }
            }
        }
    }

    public void Collecting(string name)
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

    public GameObject RaycastTag(string mask)
    {
        if (mask != "")
        {
            if (Physics.Raycast(transform.position + new Vector3(0,2,0), transform.forward, out hit, 3f, LayerMask.GetMask(mask)))
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.forward, out hit, 3f))
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }
    }

    public void CreateInfo(Vector3 pos, string txt)
    {
        GameObject infoNew = Instantiate(info, pos, info.transform.rotation);
        infoNew.transform.LookAt(cam.transform.position);

        TextMeshPro text = infoNew.transform.GetChild(0).GetComponent<TextMeshPro>();
        text.text = txt;
    }

    public void ResetBoolList(Blueprint bp)
    {
        for (int i = 0; i < bp.blueprintType.required.Count; i++)
        {
            if (bp.blueprintType.required[i].count > 0)
            {
                switch (bp.blueprintType.required[i].name)
                {
                    case "Money":
                        reqBoolList.money = true;
                        reqNameCountList.money = i;
                        break;
                }
            }
        }
    }

    public IEnumerator Exchange(Transform area, int x)
    {
        exchange = true;
        if (stackZone.transform.childCount > 1)
        {
            for (int i = stackZone.transform.childCount - 1; i >= 1; i--)
            {
                GameObject lastStack = stackZone.transform.GetChild(i).gameObject;
                lastStack.transform.DOMove(area.position, 0.25f).OnComplete(() =>
                {
                    Destroy(lastStack);
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject newObj = Instantiate(Resources.Load<GameObject>("Money"), area.position + new Vector3(0, 1, 0), area.rotation);
                        newObj.GetComponent<Rigidbody>().AddExplosionForce(500f, area.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                        newObj.GetComponent<Money>().money = 10 * x;
                    }
                });
                collectingStackCount--;
                totalExchangeHead++;
                myItems.head++;
                yield return new WaitForSeconds(0.005f);
            }
        }

        AI ai = UIActor.Instance.ai.transform.GetChild(0).GetComponent<AI>();
        if (ai.stackZone.transform.childCount > 1)
        {
            for (int i = ai.stackZone.transform.childCount - 1; i > 1; i--)
            {
                GameObject lastStack = ai.stackZone.transform.GetChild(i).gameObject;
                lastStack.transform.DOMove(area.position, 0.25f).OnComplete(() =>
                {
                    Destroy(lastStack);
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject newObj = Instantiate(Resources.Load<GameObject>("Money"), area.position + new Vector3(0, 1, 0), area.rotation);
                        newObj.GetComponent<Rigidbody>().AddExplosionForce(500f, area.position + new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 10f), Random.Range(-10f, 10f)), 50f, 7.5f);
                    }
                });
                ai.collectingStackCount--;
                yield return new WaitForSeconds(0.005f);
            }
        }
        collectingStackCount = 0;
        ai.collectingStackCount = 0;
        yield return new WaitForSeconds(1f);
        exchange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Blueprint":
                Blueprint bp = other.GetComponent<Blueprint>();

                ResetBoolList(bp);
                break;
            case "Base":
                inBase = true;
                break;
            case "Head":
                if (!maxStack || unlimitedStorage)
                {
                    if (lastHead != other.gameObject)
                    {
                        Collecting(other.name);
                        Destroy(other.gameObject);
                    }
                    lastHead = other.gameObject;
                }
                break;
            case "Money":
                myItems.money += other.GetComponent<Money>().money * (earnMoneyValue / 10f);
                Destroy(other.gameObject);
                break;
            case "Key":
                Destroy(other.gameObject);
                myItems.key += 1f;
                UIActor.Instance.keyPanel.GetComponent<Animator>().Play("key");
                UIActor.Instance.keys.fillAmount = myItems.key / 3f;
                break;
            case "Exchange":
                StartCoroutine(Exchange(other.transform, 1));
                break;
            case "Ground":
                area = other.gameObject;
                break;
            case "Health":
                if (health + (maxHealth * 0.25f) <= maxHealth)
                    health += maxHealth * 0.25f;
                else
                    health = maxHealth;
                lastHealth = health;
                Destroy(other.gameObject);
                break;
            case "Skill":
                if (!transform.parent)
                    inSkillArea = other.gameObject;
                else
                    inSkillArea = other.transform.gameObject;
                break;
            case "Arrow":
                health -= 1;
                Destroy(other.gameObject);
                break;
            case "Rewarded":
                UIActor.Instance.mainPanel.transform.Find(other.GetComponent<BoostObject>().name).GetComponent<Image>().enabled = true;
                UIActor.Instance.mainPanel.transform.Find(other.GetComponent<BoostObject>().name).GetComponent<Button>().enabled = true;

                switch (other.GetComponent<BoostObject>().name)
                {
                    case "Base":
                        AdManager.Instance.home++;
                        break;
                    case "Damage":
                        AdManager.Instance.damage++;
                        break;
                    case "Storage":
                        AdManager.Instance.storage++;
                        break;
                    case "Head":
                        AdManager.Instance.speed++;
                        break;
                }

                BoostManager.Instance.Save();

                Destroy(other.gameObject);
                break;
            case "Gurbuz":
                if (other.GetComponent<SkillArea>().inSkill)
                {
                    health -= other.GetComponent<SkillArea>().damage;

                    transform.parent.GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent.GetComponent<JoystickPlayerExample>().Invoke("isKinematicEnable", 0.5f);
                    transform.parent.GetComponent<Rigidbody>().AddForce(-other.gameObject.transform.up * 1500f);
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Blueprint":
                if (reqCoolDown <= 0)
                {
                    if (other.GetComponent<Blueprint>())
                    {
                        Blueprint bp = other.GetComponent<Blueprint>();

                        if (!bp.blueprintType.open && bp.blueprintType.canUnlock)
                        {
                            if (reqBoolList.money && myItems.money > 0)
                            {
                                myItems.money -= 5;
                                bp.blueprintType.required[reqNameCountList.money].count -= 5;

                                GameObject newObj = Instantiate(Resources.Load<GameObject>("Money"), transform.position + new Vector3(0, 2, 0), transform.rotation);
                                newObj.tag = "Untagged";
                                newObj.transform.DOMove(bp.transform.GetChild(0).GetChild(1).position, 0.2f).OnComplete(() => 
                                {
                                    Destroy(newObj);
                                });

                                reqCoolDown = 0.0001f;
                                ResetBoolList(bp);
                            }
                            bp.Control();
                        }
                    }
                }
                else
                {
                    reqCoolDown -= Time.deltaTime;
                }
                break;
            case "Fence":
                if (!other.GetComponent<Fence>().opened && other.GetComponent<Fence>().zombieArea.totalZombie.Count <= 0)
                {
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    timeBarMax = 1f;
                    UIActor.Instance.lockImage.gameObject.SetActive(true);

                    timeBar += Time.deltaTime;
                    if (timeBar >= 1)
                    {
                        other.transform.GetChild(0).DOLocalRotate(new Vector3(0, 220, 0), 0.5f);
                        other.GetComponent<Fence>().opened = true;
                        timeBar = 1;
                        other.transform.GetChild(3).gameObject.SetActive(false);
                        Invoke("TimeBarClose", 1f);
                    }
                }
                break;
            case "Weapon Upgrade":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].open && !UIActor.Instance.wpUpgradePanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.tiers.gameObject.SetActive(true);
                        UIActor.Instance.wpUpgradePanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.wpUpgCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);
                        UpgradeManager.Instance.playerLastPos = transform.position;
                        transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                        transform.parent.DOMove(UIActor.Instance.wpUpgCam.transform.GetChild(0).position, 1f);
                        timeBar = 0;
                        panel = true;
                        wpPanel = true;
                        UIActor.Instance.joystickPanel.SetActive(false);
                    }
                }
                break;
            case "Utility Upgrade":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Utility")].open && !UIActor.Instance.utilityUpgradePanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.tiers.gameObject.SetActive(true);
                        UIActor.Instance.utilityUpgradePanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.utilityUpgCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);
                        UpgradeManager.Instance.playerLastPos = transform.position;
                        transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                        transform.parent.DOMove(UIActor.Instance.utilityUpgCam.transform.GetChild(0).position, 1f);
                        timeBar = 0;
                        panel = true;
                        wpPanel = true;
                        UIActor.Instance.joystickPanel.SetActive(false);
                    }
                }
                break;
            case "Character Upgrade":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Character")].open && !UIActor.Instance.characterUpgradePanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.tiers.gameObject.SetActive(true);
                        UIActor.Instance.characterUpgradePanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.charUpgCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);
                        UpgradeManager.Instance.playerLastPos = transform.position;
                        transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                        transform.parent.DOMove(UIActor.Instance.charUpgCam.transform.GetChild(0).position, 1f);
                        timeBar = 0;
                        panel = true;
                        wpPanel = true;
                        UIActor.Instance.joystickPanel.SetActive(false);
                    }
                }
                break;
            case "Pet Shop":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Pet")].open && !LevelManager.Instance.bonusLevel && !UIActor.Instance.petUpgradePanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.petUpgradePanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.petUpgCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);

                        if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Pet")].level > 0)
                        {
                            GameObject ai = GameObject.FindObjectOfType<AnimalAI>().gameObject;
                            UpgradeManager.Instance.aiLastPos = ai.transform.position;
                            ai.transform.GetComponent<Rigidbody>().isKinematic = true;
                            ai.transform.DOMove(UIActor.Instance.petUpgCam.transform.GetChild(0).position, 1f);
                            ai.GetComponent<AnimalAI>().petPanel = true;
                        }

                        timeBar = 0;
                        panel = true;
                    }
                }
                break;
            case "Companion Upgrade":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Companion")].open && !LevelManager.Instance.bonusLevel && !UIActor.Instance.companionUpgradePanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.companionUpgradePanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.companionUpgCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);

                        if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Companion")].level > 0)
                        {
                            GameObject ai = GameObject.FindObjectOfType<AI>().gameObject;
                            UpgradeManager.Instance.aiLastPos = ai.transform.parent.position;
                            ai.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                            ai.transform.parent.DOMove(UIActor.Instance.companionUpgCam.transform.GetChild(0).position, 1f);
                            ai.GetComponent<AI>().compPanel = true;
                        }

                        panel = true;
                        timeBar = 0;
                    }
                }
                break;
            case "Doctor":
                if (!UIActor.Instance.doctorPanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.doctorPanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.doctorCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);

                        UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Doctor")].cost = (int)(maxHealth - (int)health);
                        UIActor.Instance.doctorHealthText.text = $"%{(int)((health / maxHealth) * 100)}";
                        UIActor.Instance.doctorCostText.text = $"{UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Doctor")].cost}";

                        if (health >= maxHealth)
                            UIActor.Instance.doctorRestoreMoneyButton.interactable = false;
                        else
                            UIActor.Instance.doctorRestoreMoneyButton.interactable = true;

                        panel = true;
                        timeBar = 0;
                    }
                }
                break;
            case "Magic Shop":
                if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Magic")].open && !LevelManager.Instance.bonusLevel && !UIActor.Instance.skillsPanel.activeSelf && !panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        UIActor.Instance.skillsPanel.SetActive(true);
                        UIActor.Instance.timeBar.gameObject.SetActive(false);
                        UIActor.Instance.magicCam.SetActive(true);
                        UIActor.Instance.playerCam.SetActive(false);
                        UIActor.Instance.joystickPanel.SetActive(false);

                        PlayerSkillManager.Instance.Control();

                        timeBar = 0;
                        panel = true;
                    }
                }
                break;
            case "Finish":
                if (!panel && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Vertical == 0 && transform.parent.GetComponent<JoystickPlayerExample>().variableJoystick.Horizontal == 0)
                {
                    timeBar += Time.deltaTime;
                    timeBarMax = 0.5f;
                    UIActor.Instance.timeBar.gameObject.SetActive(true);
                    if (timeBar >= timeBarMax)
                    {
                        LevelManager.Instance.StartCoroutine(LevelManager.Instance.NextLevel());
                        panel = true;
                    }
                }
                break;
            case "Skill":
                if (other.transform.parent)
                {
                    inSkillArea = other.transform.parent.gameObject;
                }
                else
                {
                    inSkillArea = other.gameObject;
                }

                if (other.GetComponent<SkillArea>())
                {
                    hasarCoolDown += Time.deltaTime;
                    if(hasarCoolDown > other.GetComponent<SkillArea>().hasarCoolDown)
                    {
                        hasarCoolDown = 0;
                        health -= other.GetComponent<SkillArea>().damage;
                    }
                }
                break;
        }
    }

    public void TimeBarClose()
    {
        UIActor.Instance.timeBar.gameObject.SetActive(false);
        UIActor.Instance.lockImage.gameObject.SetActive(false);
        timeBar = 0;
    }
    public void DismissTrader()
    {
        UIActor.Instance.traderPanel.SetActive(false);
        UIActor.Instance.trader.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Base":
                inBase = false;
                break;
            case "Fence":
                UIActor.Instance.timeBar.gameObject.SetActive(false);
                timeBar = 0;
                break;
            case "Weapon Upgrade":
                if (!panel)
                {
                    UIActor.Instance.wpUpgradePanel.SetActive(false);
                    UIActor.Instance.timeBar.gameObject.SetActive(false);
                    UIActor.Instance.tiers.gameObject.SetActive(false);
                    timeBar = 0;
                    Time.timeScale = 1;
                }
                break;
            case "Utility Upgrade":
                if (!panel)
                {
                    UIActor.Instance.utilityUpgradePanel.SetActive(false);
                    UIActor.Instance.timeBar.gameObject.SetActive(false);
                    UIActor.Instance.tiers.gameObject.SetActive(false);
                    timeBar = 0;
                    Time.timeScale = 1;
                }
                break;
            case "Character Upgrade":
                if (!panel)
                {
                    UIActor.Instance.characterUpgradePanel.SetActive(false);
                    UIActor.Instance.timeBar.gameObject.SetActive(false);
                    UIActor.Instance.tiers.gameObject.SetActive(false);
                    timeBar = 0;
                    Time.timeScale = 1;
                }
                break;
            case "Companion Upgrade":
                UIActor.Instance.companionUpgradePanel.SetActive(false);
                UIActor.Instance.timeBar.gameObject.SetActive(false);
                timeBar = 0;
                Time.timeScale = 1;
                panel = false;
                break;
            case "Pet Shop":
                UIActor.Instance.petUpgradePanel.SetActive(false);
                UIActor.Instance.timeBar.gameObject.SetActive(false);
                timeBar = 0;
                Time.timeScale = 1;
                panel = false;
                break;
            case "Ground":
                area = null;
                break;
            case "Doctor":
                UIActor.Instance.doctorPanel.SetActive(false);
                UIActor.Instance.timeBar.gameObject.SetActive(false);
                timeBar = 0;
                Time.timeScale = 1;
                panel = false;
                break;
            case "Skill":
                inSkillArea = null;
                break;
            case "Finish":
                UIActor.Instance.timeBar.gameObject.SetActive(false);
                timeBar = 0;
                Time.timeScale = 1;
                panel = false;
                break;
        }
    }

    public void Retry()
    {
        health = maxHealth;
        SetRagdoll(false);
        dead = false;
        transform.parent.position = new Vector3(9, 0, -20);

        for (int i = 1; i < stackZone.transform.childCount; i++)
        {
            Destroy(stackZone.transform.GetChild(i).gameObject);
        }

        GameObject[] heads = GameObject.FindGameObjectsWithTag("Head");
        foreach (GameObject head in heads)
        {
            Destroy(head.gameObject);
        }

        GameObject[] healths = GameObject.FindGameObjectsWithTag("Health");
        foreach (GameObject health in healths)
        {
            Destroy(health.gameObject);
        }

        ZombieManager[] rooms = GameObject.FindObjectsOfType<ZombieManager>();
        foreach (ZombieManager room in rooms)
        {
            room.spawnTime = 0;
        }
    }

    public void Revive()
    {
        health = maxHealth;
        SetRagdoll(false);
        dead = false;
    }

    public void GoHome()
    {
        SetRagdoll(false);
        dead = false;
        transform.parent.position = new Vector3(9, 0, -20);
    }

    public void Dead()
    {
        SetRagdoll(true);
        dead = true;

        string[] hintList = { "Upgrade Your Weapon!", "Upgrade Your Character!", "Upgrade Your Skills!" };
        hint = hintList[Random.Range(0, hintList.Length)];

        deadTimer = 10f;
    }


    bool turn = false;
    public void CanYanipSonme(Image image)
    {
        if (!turn)
        {
            if (image.color.a < 1f)
            {
                image.color += new Color(0, 0, 0, Time.deltaTime);
            }
            else
            {
                turn = true;
            }
        }
        else
        {
            if (image.color.a > 0f)
            {
                image.color -= new Color(0, 0, 0, Time.deltaTime);
            }
            else
            {
                turn = false;
            }
        }
    }
    bool turn2 = false;
    public void StackYanipSonme(Image image)
    {
        if (!turn2)
        {
            if (image.color.a < 1f)
            {
                image.color += new Color(0, 0, 0, Time.deltaTime);
            }
            else
            {
                turn2 = true;
            }
        }
        else
        {
            if (image.color.a > 0f)
            {
                image.color -= new Color(0, 0, 0, Time.deltaTime);
            }
            else
            {
                turn2 = false;
            }
        }
    }

    public void SetRagdoll(bool active)
    {
        GetComponent<Collider>().enabled = !active;
        GetComponent<Animator>().enabled = !active;

        Rigidbody[] rigidbodies = transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = !active;
        }

        Collider[] colliders = transform.GetChild(0).GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = active;
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
        lastHealth = health;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat($"Money", myItems.money);
        PlayerPrefs.SetInt($"Head", myItems.head);

        PlayerPrefs.SetInt("Level", LevelManager.Instance.levelCount);

        for (int i = 0; i < UpgradeManager.Instance.UpgradeLists.Count; i++)
        {
            PlayerPrefs.SetInt($"UpgradeLevel {i}", UpgradeManager.Instance.UpgradeLists[i].level);
            PlayerPrefs.SetInt($"UpgradeCost {i}", UpgradeManager.Instance.UpgradeLists[i].cost);
            PlayerPrefs.SetInt($"UpgradeProcess {i}", UpgradeManager.Instance.UpgradeLists[i].process);
            PlayerPrefs.SetString($"UpgradeOpen {i}", UpgradeManager.Instance.UpgradeLists[i].open.ToString());
        }

        for (int i = 0; i < PlayerSkillManager.Instance.skills.Count; i++)
        {
            PlayerPrefs.SetInt($"SkillLevel {i}", PlayerSkillManager.Instance.skills[i].level);
            PlayerPrefs.SetInt($"SkillCost {i}", (int)PlayerSkillManager.Instance.skills[i].cost);
            PlayerPrefs.SetInt($"SkillProcess {i}", PlayerSkillManager.Instance.skills[i].process);
            PlayerPrefs.SetInt($"SkillDamage {i}", (int)PlayerSkillManager.Instance.skills[i].damage);
            PlayerPrefs.SetString($"SkillOpen {i}", PlayerSkillManager.Instance.skills[i].open.ToString());
            PlayerPrefs.SetString($"SkillSelected {i}", PlayerSkillManager.Instance.skills[i].selected.ToString());
        }
        for (int i = 0; i < skills.Length; i++)
        {
            PlayerPrefs.SetInt($"MySkillValue {i}", skills[i]);
        }

        PlayerPrefs.SetFloat("PlayerHealth", maxHealth);
        PlayerPrefs.SetInt("PlayerStack", maxStackCount);
        PlayerPrefs.SetInt("PlayerArmor", armor);
        PlayerPrefs.SetFloat("PlayerMoney", earnMoneyValue);
        PlayerPrefs.SetFloat("PlayerSpeed", transform.parent.GetComponent<JoystickPlayerExample>().speed);

        PlayerPrefs.SetInt("HomeRW", AdManager.Instance.home);
        PlayerPrefs.SetInt("DamageRW", AdManager.Instance.damage);
        PlayerPrefs.SetInt("StorageRW", AdManager.Instance.storage);
        PlayerPrefs.SetInt("HeadRW", AdManager.Instance.speed);
    }

    public void Load()
    {
        myItems.money = PlayerPrefs.GetFloat("Money");
        myItems.head = PlayerPrefs.GetInt("Head");

        LevelManager.Instance.levelCount = PlayerPrefs.GetInt("Level");

        for (int i = 0; i < UpgradeManager.Instance.UpgradeLists.Count; i++)
        {
            UpgradeManager.Instance.UpgradeLists[i].level = PlayerPrefs.GetInt($"UpgradeLevel {i}");
            UpgradeManager.Instance.UpgradeLists[i].cost = PlayerPrefs.GetInt($"UpgradeCost {i}");
            UpgradeManager.Instance.UpgradeLists[i].process = PlayerPrefs.GetInt($"UpgradeProcess {i}");

            if (PlayerPrefs.GetString($"UpgradeOpen {i}") == "True")
                UpgradeManager.Instance.UpgradeLists[i].open = true;
            else
                UpgradeManager.Instance.UpgradeLists[i].open = false;
        }

        if(PlayerPrefs.HasKey("MySkillValue 0"))
        {
            for (int i = 0; i < PlayerSkillManager.Instance.skills.Count; i++)
            {
                PlayerSkillManager.Instance.skills[i].level = PlayerPrefs.GetInt($"SkillLevel {i}");
                PlayerSkillManager.Instance.skills[i].cost = PlayerPrefs.GetInt($"SkillCost {i}");
                PlayerSkillManager.Instance.skills[i].process = PlayerPrefs.GetInt($"SkillProcess {i}");
                PlayerSkillManager.Instance.skills[i].damage = PlayerPrefs.GetInt($"SkillDamage {i}");

                if (PlayerPrefs.GetString($"SkillOpen {i}") == "True")
                    PlayerSkillManager.Instance.skills[i].open = true;
                else
                    PlayerSkillManager.Instance.skills[i].open = false;
            }

            for (int i = 0; i < skills.Length; i++)
            {
                skills[i] = PlayerPrefs.GetInt($"MySkillValue {i}");

                if(skills[i] != 10)
                PlayerSkillManager.Instance.Select(skills[i]);
            }
        }

        maxHealth = PlayerPrefs.GetFloat("PlayerHealth");
        maxStackCount = PlayerPrefs.GetInt("PlayerStack");
        armor = PlayerPrefs.GetInt("PlayerArmor");
        earnMoneyValue = PlayerPrefs.GetFloat("PlayerMoney");
        transform.parent.GetComponent<JoystickPlayerExample>().speed = PlayerPrefs.GetFloat("PlayerSpeed");

        AdManager.Instance.home = PlayerPrefs.GetInt("HomeRW");
        AdManager.Instance.damage = PlayerPrefs.GetInt("DamageRW");
        AdManager.Instance.storage = PlayerPrefs.GetInt("StorageRW");
        AdManager.Instance.speed = PlayerPrefs.GetInt("HeadRW");
    }
}
