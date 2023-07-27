using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using CrazyGames;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int levelCount;
    public GameObject environment;
    public GameObject truck;
    public bool truckComing;

    public GameObject lastLevel;

    public List<Color> skyboxies;
    public List<Color> fog;

    public List<GameObject> walkableAreas;
    public float playTime;

    public float intersDelay, maxintersDelay;
    public float intersCap, maxintersCap;

    public GameObject bonusLevel;

    public GameObject[] allAreas;
    GameObject lastArea;

    public NavMeshSurface[] surfaces;

    Player player;

    [HideInInspector] public bool start;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxintersCap = intersCap;
        maxintersDelay = intersDelay;

        int level = 0;
        if (levelCount % 5 != 0)
        {
            level = levelCount - (5 * (levelCount / 5)) - 1;
        }
        else
        {
            level = levelCount - (5 * ((levelCount / 5) - 1)) - 1;
        }

        UIActor.Instance.swipeScreen.SetActive(true);
        lastLevel = Instantiate(environment.transform.GetChild(level).gameObject);
        lastLevel.SetActive(true);
        RenderSettings.ambientSkyColor = skyboxies[level];
        RenderSettings.fogColor = fog[level];

        player = GameObject.FindObjectOfType<Player>();

        if (PlayerPrefs.HasKey("playtime"))
            playTime = PlayerPrefs.GetFloat("playtime");

        if (PlayerPrefs.HasKey("intersDelay"))
            intersDelay = PlayerPrefs.GetFloat("intersDelay");

        if (maxintersDelay < intersDelay)
            intersDelay = maxintersDelay;

        UIActor.Instance.levelPanelText.text = "LEVEL " + levelCount.ToString();

        NavMeshBuild();
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        PlayerPrefs.SetFloat("playtime", playTime);
        PlayerPrefs.SetFloat("intersDelay", intersDelay);

        if (intersDelay > 0)
            intersDelay -= Time.deltaTime;

        if (intersDelay <= 0)
        {
            if (intersCap > 0)
            {
                if (!bonusLevel)
                {
                    intersCap -= Time.deltaTime;
                }
            }
            else
            {
                if (player.area && player.area.GetComponent<ZombieManager>().totalZombie.Count <= 0 || !player.area)
                {
                    AdManager.Instance.ShowInterstial();
                }
            }
        }

        if (!start && !UIActor.Instance.levelGecis.gameObject.activeSelf && UIActor.Instance.swipeScreen.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UIActor.Instance.swipeScreen.SetActive(false);
                start = true;

                CrazyEvents.Instance.GameplayStart();
            }
        }
    }

    public void TruckComing()
    {
        truck.transform.DOMove(lastLevel.GetComponent<LevelActor>().truck.transform.position, 2f).OnComplete(() => 
        {
            truck.transform.GetChild(0).gameObject.SetActive(true);
        });
        truckComing = true;
    }

    public void Spin()
    {
        GameObject.FindObjectOfType<CarControl>().carkifelekB = true;
        GameObject.FindObjectOfType<CarControl>().randomSpeed = Random.Range(360f, 720f);
        UIActor.Instance.spinButton.SetActive(false);
    }

    public void SpinNext()
    {
        GameObject.FindObjectOfType<CarControl>().Next();
    }

    public IEnumerator BossPanel(int level)
    {
        yield return new WaitForSeconds(2f);

        List<int> newWeapons = new List<int>();

        for (int i = 0; i < WeaponManager.Instance.weapons.Count; i++)
        {
            if (WeaponManager.Instance.weapons[i].level == levelCount + 1)
            {
                newWeapons.Add(i);
            }
        }

        if (newWeapons.Count > 0)
        {
            for (int i = 0; i < newWeapons.Count; i++)
            {
                switch (newWeapons[i] / 10)
                {
                    case 0:
                        UIActor.Instance.gunImages[i].color = new Color(0.75f, 0.75f, 0.75f, 1);
                        break;
                    case 1:
                        UIActor.Instance.gunImages[i].color = new Color(0, 0.75f, 1, 1);
                        break;
                    case 2:
                        UIActor.Instance.gunImages[i].color = new Color(0.5f, 0.15f, 1, 1);
                        break;
                    case 3:
                        UIActor.Instance.gunImages[i].color = new Color(0.96f, 0.75f, 0.1f, 1);
                        break;
                }

                UIActor.Instance.gunImages[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Gun {(newWeapons[i] % 10) + 1}");
            }
        }
        else
        {
            UIActor.Instance.gunImages[0].transform.parent.gameObject.SetActive(false);
        }

        UIActor.Instance.boss1Image.sprite = Resources.Load<Sprite>($"BossResimleri/Boss{level}");
        UIActor.Instance.boss2Image.sprite = Resources.Load<Sprite>($"BossResimleri/Boss{(level + 1 > 10 ? 1 : level + 1)}");
        UIActor.Instance.bossPanel.SetActive(true);
    }

    public void NavMeshBuild()
    {
        surfaces = GameObject.FindObjectsOfType<NavMeshSurface>();

        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    public IEnumerator NextLevel()
    {
        CrazyEvents.Instance.GameplayStop();

        playTime = 0;
        UIActor.Instance.levelGecis.gameObject.SetActive(true);
        while (UIActor.Instance.levelGecis.color.a < 1f)
        {
            UIActor.Instance.levelGecis.color += new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForFixedUpdate();
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

        yield return new WaitForSeconds(0.1f);

        Player player = GameObject.FindObjectOfType<Player>();
        player.health = player.maxHealth;

        GameObject ai = null;
        if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Companion")].level > 0)
            ai = GameObject.FindObjectOfType<AI>().gameObject;

        GameObject dog = null;
        if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Pet")].level > 0)
            dog = GameObject.FindObjectOfType<AnimalAI>().gameObject;

        truckComing = false;

        levelCount++;

        player.transform.parent.position = new Vector3(9, 0, -20);
        truck.transform.position = new Vector3(-10f, 0, 400f);
        truck.transform.GetChild(0).gameObject.SetActive(false);

        UIActor.Instance.timeBar.gameObject.SetActive(false);
        player.timeBar = 0;

        int level = 0;
        if (levelCount % 5 != 0)
        {
            level = levelCount - (5 * (levelCount / 5)) - 1;
        }
        else
        {
            level = levelCount - (5 * ((levelCount / 5) - 1)) - 1;
        }

        UIActor.Instance.swipeScreen.SetActive(true);
        start = false;

        Destroy(lastLevel);
        lastLevel = Instantiate(environment.transform.GetChild(level).gameObject);
        lastLevel.SetActive(true);
        RenderSettings.ambientSkyColor = skyboxies[level];
        RenderSettings.fogColor = fog[level];

        ZombieManager[] zombieAreas = GameObject.FindObjectsOfType<ZombieManager>();
        foreach(ZombieManager area in zombieAreas)
        {
            area.spawnerTime = 90;
        }

        if (level == 0 && levelCount > 1)
            TutorialManager.Instance.Invoke("FenceKapat", 0.2f);

        //COMPANIONS
        for (int i = 0; i < CompanionManager.Instance.companions.Count; i++)
        {
            if (CompanionManager.Instance.companions[i].level == levelCount)
            {
                GameObject companion = Instantiate(CompanionManager.Instance.companions[i].companion, player.transform.position + (player.transform.right * 1), player.transform.rotation);
                Companion comp = companion.GetComponent<Companion>();
                comp.go = true;
                if (CompanionManager.Instance.companions[i].area)
                {
                    comp.target = CompanionManager.Instance.companions[i].area;
                    companion.GetComponent<NavMeshAgent>().SetDestination(comp.target != null ? comp.target.transform.position : new Vector3(-10, 0, 10));
                }
                comp.comps.openingObjects = CompanionManager.Instance.companions[i].openingObjects;
                comp.comps.closingObjects = CompanionManager.Instance.companions[i].closingObjects;
            }
        }

        UIActor.Instance.levelPanelText.text = "LEVEL " + levelCount.ToString();

        NavMeshBuild();

        while (UIActor.Instance.levelGecis.color.a > 0f)
        {
            UIActor.Instance.levelGecis.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        UIActor.Instance.levelGecis.gameObject.SetActive(false);
        player.panel = false;
    }

    public IEnumerator LevelGecis()
    {
        UIActor.Instance.levelGecis.gameObject.SetActive(true);
        while (UIActor.Instance.levelGecis.color.a < 1f)
        {
            UIActor.Instance.levelGecis.color += new Color(0, 0, 0, Time.deltaTime * 5f);
            yield return new WaitForFixedUpdate();
        }

        UIActor.Instance.mainCam.SetActive(false);

        RenderSettings.ambientSkyColor = Color.white;
        RenderSettings.fogColor = Color.white;

        while (UIActor.Instance.levelGecis.color.a > 0f)
        {
            UIActor.Instance.levelGecis.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        UIActor.Instance.levelGecis.gameObject.SetActive(false);
    }
}
