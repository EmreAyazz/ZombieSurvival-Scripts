using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrazyGames;
using static UnityEngine.Advertisements.Advertisement;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    //public AdsInitializer adsInitializer;

    public GameObject homeButton, damageButton, storageButton, speedButton;
    public Sprite homeVideo, damageVideo, storageVideo, speedVideo;
    public Sprite homeUse, damageUse, storageUse, speedUse;

    public int home, damage, speed, storage;

    public float damageTime, storageTime, speedTime;

    public TextMeshProUGUI damageTimeText, storageTimeText, speedTimeText;
    public Text damageCountText, storageCountText, speedCountText, homeCountText;

    public bool shownDamage, shownHead, shownCapacity, shownHome;

    Player player;

    public bool forcedNet = true;
    public GameObject netReqPanel;

    public CrazyBanner crazyBanner;

    string adName;
    int skillValue;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (damageTime <= 0)
            {
                player.x2Damage = false;
                damageTimeText.text = "";
                damageCountText.enabled = true;
                damageButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                player.x2Damage = true;
                damageTime -= Time.deltaTime;
                damageTimeText.text = damageTime > 10 ? $"00:{(int)damageTime}" : $"00:0{(int)damageTime}";
                damageCountText.enabled = false;
                damageButton.GetComponent<Button>().interactable = false;
            }

            if (speedTime <= 0)
            {
                player.x2Head = false;
                speedTimeText.text = "";
                speedCountText.enabled = true;
                speedButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                player.x2Head = true;
                speedTime -= Time.deltaTime;
                speedTimeText.text = speedTime > 10 ? $"00:{(int)speedTime}" : $"00:0{(int)speedTime}";
                speedCountText.enabled = false;
                speedButton.GetComponent<Button>().interactable = false;
            }

            if (storageTime <= 0)
            {
                player.unlimitedStorage = false;
                storageTimeText.text = "";
                storageButton.GetComponent<Button>().interactable = true;
                storageCountText.enabled = true;
            }
            else
            {
                player.unlimitedStorage = true;
                storageTime -= Time.deltaTime;
                storageTimeText.text = storageTime > 10 ? $"00:{(int)storageTime}" : $"00:0{(int)storageTime}";
                storageCountText.enabled = false;
                storageButton.GetComponent<Button>().interactable = false;
            }



            if (damage > 0 || damageTime > 0)
            {
                damageButton.GetComponent<Image>().enabled = true;
                damageButton.GetComponent<Button>().enabled = true;
                damageButton.GetComponent<Image>().sprite = damageUse;
                damageCountText.text = $"{damage}";
                shownDamage = false;
            }
            else
            {
                damageButton.GetComponent<Image>().sprite = damageVideo;
                damageCountText.text = "";
            }

            if (speed > 0 || speedTime > 0)
            {
                speedButton.GetComponent<Image>().enabled = true;
                speedButton.GetComponent<Button>().enabled = true;
                speedButton.GetComponent<Image>().sprite = speedUse;
                speedCountText.text = $"{speed}";
                shownHead = false;
            }
            else
            {
                speedButton.GetComponent<Image>().sprite = speedVideo;
                speedCountText.text = "";
            }

            if (storage > 0 || storageTime > 0)
            {
                storageButton.GetComponent<Image>().enabled = true;
                storageButton.GetComponent<Button>().enabled = true;
                storageButton.GetComponent<Image>().sprite = storageUse;
                storageCountText.text = $"{storage}";
                shownCapacity = false;
            }
            else
            {
                storageButton.GetComponent<Image>().sprite = storageVideo;
                storageCountText.text = "";
            }

            if (home > 0)
            {
                homeButton.GetComponent<Image>().enabled = true;
                homeButton.GetComponent<Button>().enabled = true;
                homeButton.GetComponent<Image>().sprite = homeUse;
                homeCountText.text = $"{home}";
                shownHome = false;
            }
            else
            {
                homeButton.GetComponent<Image>().sprite = homeVideo;
                homeCountText.text = "";
            }
        }
    }

    public void ShowInterstial()
    {
        Time.timeScale = 0;
        CrazyAds.Instance.beginAdBreak(CompletedInterstitial, CompletedInterstitial);

        LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
    }

    public void CompletedRewarded()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        Time.timeScale = 1;

        switch (adName)
        {
            case "GoingHome":
                home++;
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "DamageX2":
                damage++;
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "SpeedX2":
                speed++;
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "UnlimitedStorage":
                storage++;
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "Revive":
                player.Revive();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "DoctorRewardedRestore":
                UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.DoctorRewardedRestore());
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "ArmorUpgrade":
                UpgradeManager.Instance.ArmorUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "HealthUpgrade":
                UpgradeManager.Instance.HealthUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "SpeedUpgrade":
                UpgradeManager.Instance.SpeedUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "StorageUpgrade":
                UpgradeManager.Instance.CapacityUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "MoneyUpgrade":
                UpgradeManager.Instance.MoneyUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "WpUpgrade":
                UpgradeManager.Instance.WeaponUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "CompanionUpgrade":
                UpgradeManager.Instance.CompanionUpgradeRW();
                LevelManager.Instance.intersCap = LevelManager.Instance.maxintersCap;
                break;
            case "Trader":
                player.StartCoroutine(player.Exchange(UIActor.Instance.trader.transform, 1));

                UIActor.Instance.trader.SetActive(false);
                UIActor.Instance.traderPanel.SetActive(false);
                Time.timeScale = 1;
                break;
            case "BuySkinWithRewarded":
                player.transform.GetChild(SkinManager.Instance.currentValue).gameObject.SetActive(false);
                player.transform.GetChild(SkinManager.Instance.value).gameObject.SetActive(true);
                SkinManager.Instance.currentValue = SkinManager.Instance.value;

                SkinManager.Instance.mySkins.Add(SkinManager.Instance.value);

                SkinManager.Instance.select.gameObject.SetActive(true);
                SkinManager.Instance.select.interactable = false;
                SkinManager.Instance.select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
                SkinManager.Instance.buy.gameObject.SetActive(false);
                SkinManager.Instance.rewarded.gameObject.SetActive(false);

                SkinManager.Instance.Save();
                break;
            case "SkillUpgradeWithRW":
                if (PlayerSkillManager.Instance.skills[skillValue].level == 0)
                {
                    PlayerSkillManager.Instance.skills[skillValue].level += 1;
                    PlayerSkillManager.Instance.skills[skillValue].cost += (PlayerSkillManager.Instance.skills[skillValue].level * 10);
                    PlayerSkillManager.Instance.skills[skillValue].open = true;
                    PlayerSkillManager.Instance.skills[skillValue].damage += 20;
                }
                else
                {
                    PlayerSkillManager.Instance.skills[skillValue].process += 1;
                    PlayerSkillManager.Instance.skills[skillValue].cost += (PlayerSkillManager.Instance.skills[skillValue].level * 10);
                    PlayerSkillManager.Instance.skills[skillValue].damage += 5;
                }
                PlayerSkillManager.Instance.Control();
                break;
            case "SpinRW":
                CarControl car = GameObject.FindObjectOfType<CarControl>();
                car.carkifelekB = true;
                car.randomSpeed = Random.Range(360f, 720f);
                car.carkOkay = false;
                car.verildi = false;
                UIActor.Instance.spinWinPanel.SetActive(false);
                break;
        }
    }
    public void CompletedInterstitial()
    {
        Time.timeScale = 1;
        //adsInitializer.LoadInterstitial();
    }

    public void GoingHome()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        if (home <= 0)
        {
            Time.timeScale = 0;
            adName = "GoingHome";
            CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
        }
        else
        {
            player.GoHome();
            home--;
        }
    }

    public void DamageX2()
    {
        if (damage <= 0)
        {
            Time.timeScale = 0;
            adName = "DamageX2";
            CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
        }
        else
        {
            damageTime = 30;
            damage--;
        }
    }
    public void SpeedX2()
    {
        if (speed <= 0)
        {
            Time.timeScale = 0;
            adName = "SpeedX2";
            CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
        }
        else
        {
            speedTime = 30;
            speed--;
        }
    }
    public void UnlimitedStorage()
    {
        if (storage <= 0)
        {
            Time.timeScale = 0;
            adName = "UnlimitedStorage";
            CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
        }
        else
        {
            storageTime = 30;
            storage--;
        }
    }
    public void Revive()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        Time.timeScale = 0;
        adName = "Revive";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void DoctorRewardedRestore()
    {
        Time.timeScale = 0;
        adName = "DoctorRewardedRestore";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }

    public void ArmorUpgrade()
    {
        Time.timeScale = 0;
        adName = "ArmorUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void HealthUpgrade()
    {
        Time.timeScale = 0;
        adName = "HealthUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void SpeedUpgrade()
    {
        Time.timeScale = 0;
        adName = "SpeedUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void StorageUpgrade()
    {
        Time.timeScale = 0;
        adName = "StorageUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void MoneyUpgrade()
    {
        Time.timeScale = 0;
        adName = "MoneyUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void WpUpgrade()
    {
        Time.timeScale = 0;
        adName = "WpUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void CompanionUpgrade()
    {
        Time.timeScale = 0;
        adName = "CompanionUpgrade";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void Trader()
    {
        Time.timeScale = 0;
        adName = "Trader";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }

    public void BuySkinWithRewarded()
    {
        Time.timeScale = 0;
        adName = "BuySkinWithRewarded";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }

    public void SkillUpgradeWithRW(int value)
    {
        Time.timeScale = 0;
        adName = "SkillUpgradeWithRW";

        skillValue = value;

        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
    public void SpinRW()
    {
        Time.timeScale = 0;
        adName = "SpinRW";
        CrazyAds.Instance.beginAdBreakRewarded(CompletedRewarded, CompletedRewarded);
    }
}
