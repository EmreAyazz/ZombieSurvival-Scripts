using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TT.Weapon;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeInfo> UpgradeLists;

    int baseCount;
    int weaponCount;
    int companionCount;
    int healthCount;
    int speedCount;
    int capacityCount;
    int moneyCount;
    int armorCount;
    int petCount;

    public Vector3 playerLastPos;
    public Vector3 aiLastPos;

    Player player;

    [System.Serializable]
    public class UpgradeInfo
    {
        public string name;
        public int level;
        public int cost;
        public bool open;
        public int process;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        baseCount = Control("Base");
        weaponCount = Control("Weapon");
        companionCount = Control("Companion");
        healthCount = Control("Health");
        speedCount = Control("Speed");
        capacityCount = Control("Capacity");
        moneyCount = Control("Money");
        armorCount = Control("Armor");
        petCount = Control("Pet");

        player = GameObject.FindObjectOfType<Player>();

        if (UpgradeLists[petCount].level > 0)
        {
            UIActor.Instance.dog.SetActive(true);
            UIActor.Instance.petBuyButton.interactable = false;
        }

        if (UpgradeLists[companionCount].level > 0)
        {
            UIActor.Instance.companionBuyButton.SetActive(false);
            UIActor.Instance.ai.SetActive(true);
            AI ai = UIActor.Instance.ai.transform.GetChild(0).GetComponent<AI>();
            ai.weaponCount = UpgradeLists[companionCount].level;
            ai.GetComponent<Animator>().SetInteger("WeaponType_int", ai.weaponCount);
            ai.GetComponent<Animator>().SetBool("Idle_b", true);
            ai.weapons.transform.GetChild(UpgradeLists[companionCount].level - 1).gameObject.SetActive(true);
            UIActor.Instance.companionSoldierOpenImage.SetActive(false);
            ai.maxStackCount = UpgradeLists[companionCount].level * 5 + UpgradeLists[companionCount].process;
            UIActor.Instance.companionUpgradeLevelImage.fillAmount = UpgradeLists[companionCount].process / 5f;
        }

        if (UpgradeLists[healthCount].level >= 10)
        {
            UIActor.Instance.healthUpgradeCostText.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.healthButtonRW.SetActive(false);
            UIActor.Instance.healthUpgradeValueText.text = $"+ {player.maxHealth} HEALTH";
            UIActor.Instance.healthUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.healthUpgradeLevelImage.fillAmount = 1;
        }
        if (UpgradeLists[speedCount].level >= 10)
        {
            UIActor.Instance.speedUpgradeCostText.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.speedButtonRW.SetActive(false);
            UIActor.Instance.speedUpgradeValueText.text = $"+ {player.transform.parent.GetComponent<JoystickPlayerExample>().speed} SPEED";
            UIActor.Instance.speedUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.speedUpgradeLevelImage.fillAmount = 1;
        }
        if (UpgradeLists[capacityCount].level >= 10)
        {
            UIActor.Instance.capacityUpgradeCostText.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.capacityButtonRW.SetActive(false);
            UIActor.Instance.capacityUpgradeValueText.text = $"+ {player.maxStackCount} CAPACITY";
            UIActor.Instance.capacityUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.capacityUpgradeLevelImage.fillAmount = 1;
        }
        if (UpgradeLists[moneyCount].level >= 10)
        {
            UIActor.Instance.moneyUpgradeCostText.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.moneyButtonRW.SetActive(false);
            UIActor.Instance.moneyUpgradeValueText.text = $"+ {player.earnMoneyValue * 3f} MONEY";
            UIActor.Instance.moneyUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.moneyUpgradeLevelImage.fillAmount = 1;
        }
        if (UpgradeLists[armorCount].level >= 10)
        {
            UIActor.Instance.armorUpgradeCostText.transform.parent.gameObject.SetActive(false);
            UIActor.Instance.armorButtonRW.SetActive(false);
            UIActor.Instance.armorUpgradeValueText.text = $"+ {player.armor} ARMOR";
            UIActor.Instance.armorUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.armorUpgradeLevelImage.fillAmount = 1;
        }
        if (UpgradeLists[companionCount].level >= 10)
        {
            UIActor.Instance.companionUpgradeCostText.gameObject.SetActive(false);
            UIActor.Instance.companionButtonRW.SetActive(false);
            UIActor.Instance.companionButton.SetActive(false);
            UIActor.Instance.companionSoldierStorageText.text = $"{UpgradeLists[companionCount].level * 5 + UpgradeLists[companionCount].process}";
            UIActor.Instance.companionUpgradeLevelText.text = $"MAX LEVEL";
            UIActor.Instance.companionUpgradeLevelImage.fillAmount = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UIActor.Instance.petUpgradeCostText.text = $"{UpgradeLists[petCount].cost}";

        if (UIActor.Instance.gunUpgradePanel.activeSelf && UIActor.Instance.wpUpgradePanel.activeSelf)
            UIActor.Instance.tiers.gameObject.SetActive(true);
        else
            UIActor.Instance.tiers.gameObject.SetActive(false);

        if (UpgradeLists[healthCount].level < 10)
        {
            if (UpgradeLists[healthCount].cost > player.myItems.money)
            {
                UIActor.Instance.healthButtonRW.SetActive(true);
                UIActor.Instance.healthButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.healthButtonRW.SetActive(false);
                UIActor.Instance.healthButton.SetActive(true);
            }
        }
        if (UpgradeLists[speedCount].level < 10)
        {
            if (UpgradeLists[speedCount].cost > player.myItems.money)
            {
                UIActor.Instance.speedButtonRW.SetActive(true);
                UIActor.Instance.speedButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.speedButtonRW.SetActive(false);
                UIActor.Instance.speedButton.SetActive(true);
            }
        }
        if (UpgradeLists[armorCount].level < 10)
        {
            if (UpgradeLists[armorCount].cost > player.myItems.money)
            {
                UIActor.Instance.armorButtonRW.SetActive(true);
                UIActor.Instance.armorButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.armorButtonRW.SetActive(false);
                UIActor.Instance.armorButton.SetActive(true);
            }
        }
        if (UpgradeLists[capacityCount].level < 10)
        {
            if (UpgradeLists[capacityCount].cost > player.myItems.money)
            {
                UIActor.Instance.capacityButtonRW.SetActive(true);
                UIActor.Instance.capacityButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.capacityButtonRW.SetActive(false);
                UIActor.Instance.capacityButton.SetActive(true);
            }
        }
        if (UpgradeLists[moneyCount].level < 10)
        {
            if (UpgradeLists[moneyCount].cost > player.myItems.money)
            {
                UIActor.Instance.moneyButtonRW.SetActive(true);
                UIActor.Instance.moneyButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.moneyButtonRW.SetActive(false);
                UIActor.Instance.moneyButton.SetActive(true);
            }
        }
        if (UpgradeLists[weaponCount].level < 40)
        {
            if (UpgradeLists[weaponCount].cost > player.myItems.money && LevelManager.Instance.levelCount * 3 >= UpgradeLists[weaponCount].level + 1)
            {
                UIActor.Instance.wpButtonRW.SetActive(true);
                UIActor.Instance.wpButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.wpButtonRW.SetActive(false);
                UIActor.Instance.wpButton.SetActive(true);
            }
            if (LevelManager.Instance.levelCount * 3 >= UpgradeLists[weaponCount].level + 1)
            {
                UIActor.Instance.wpButton.GetComponent<Button>().interactable = true;
                UIActor.Instance.wpButton.transform.GetChild(0).gameObject.SetActive(true);
                UIActor.Instance.wpButton.transform.GetChild(1).gameObject.SetActive(true);
                UIActor.Instance.wpButton.transform.GetChild(2).gameObject.SetActive(true);
                UIActor.Instance.wpButton.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                UIActor.Instance.wpButton.GetComponent<Button>().interactable = false;
                UIActor.Instance.wpButton.transform.GetChild(0).gameObject.SetActive(false);
                UIActor.Instance.wpButton.transform.GetChild(1).gameObject.SetActive(false);
                UIActor.Instance.wpButton.transform.GetChild(2).gameObject.SetActive(false);
                UIActor.Instance.wpButton.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        if (UpgradeLists[companionCount].level < 10)
        {
            if (UpgradeLists[companionCount].cost > player.myItems.money)
            {
                UIActor.Instance.companionButtonRW.SetActive(true);
                UIActor.Instance.companionButton.SetActive(false);
                UIActor.Instance.companionBuyButton.SetActive(false);
            }
            else
            {
                UIActor.Instance.companionButtonRW.SetActive(false);
                if (UpgradeLists[companionCount].level > 0)
                    UIActor.Instance.companionButton.SetActive(true);
                else
                    UIActor.Instance.companionBuyButton.SetActive(true);
            }
        }

        if (UpgradeLists[healthCount].level < 10)
        {
            UIActor.Instance.healthUpgradeCostText.text = $"{UpgradeLists[healthCount].cost}";
            UIActor.Instance.healthUpgradeLevelText.text = $"LEVEL {UpgradeLists[healthCount].level}";
            UIActor.Instance.healthUpgradeLevelImage.fillAmount = UpgradeLists[healthCount].process / 5f;

            UIActor.Instance.healthUpgradeValueText.text = $"+ {player.maxHealth} HEALTH";
        }

        if (UpgradeLists[speedCount].level < 10)
        {
            UIActor.Instance.speedUpgradeCostText.text = $"{UpgradeLists[speedCount].cost}";
            UIActor.Instance.speedUpgradeLevelText.text = $"LEVEL {UpgradeLists[speedCount].level}";
            UIActor.Instance.speedUpgradeLevelImage.fillAmount = UpgradeLists[speedCount].process / 5f;

            UIActor.Instance.speedUpgradeValueText.text = $"+ {player.transform.parent.GetComponent<JoystickPlayerExample>().speed} SPEED";
        }

        if (UpgradeLists[capacityCount].level < 10)
        {
            UIActor.Instance.capacityUpgradeCostText.text = $"{UpgradeLists[capacityCount].cost}";
            UIActor.Instance.capacityUpgradeLevelText.text = $"LEVEL {UpgradeLists[capacityCount].level}";
            UIActor.Instance.capacityUpgradeLevelImage.fillAmount = UpgradeLists[capacityCount].process / 5f;

            UIActor.Instance.capacityUpgradeValueText.text = $"+ {player.maxStackCount} CAPACITY";
        }

        if (UpgradeLists[moneyCount].level < 10)
        {
            UIActor.Instance.moneyUpgradeCostText.text = $"{UpgradeLists[moneyCount].cost}";
            UIActor.Instance.moneyUpgradeLevelText.text = $"LEVEL {UpgradeLists[moneyCount].level}";
            UIActor.Instance.moneyUpgradeLevelImage.fillAmount = UpgradeLists[moneyCount].process / 5f;

            UIActor.Instance.moneyUpgradeValueText.text = $"+ {player.earnMoneyValue * 3f} MONEY";
        }

        if (UpgradeLists[armorCount].level < 10)
        {
            UIActor.Instance.armorUpgradeCostText.text = $"{UpgradeLists[armorCount].cost}";
            UIActor.Instance.armorUpgradeLevelText.text = $"LEVEL {UpgradeLists[armorCount].level}";
            UIActor.Instance.armorUpgradeLevelImage.fillAmount = UpgradeLists[armorCount].process / 5f;

            UIActor.Instance.armorUpgradeValueText.text = $"+ {player.armor} ARMOR";
        }

        if (UpgradeLists[companionCount].level < 10)
        {
            UIActor.Instance.companionUpgradeLevelText.text = $"LEVEL {UpgradeLists[companionCount].level}";
            UIActor.Instance.companionUpgradeCostText.text = $"{UpgradeLists[companionCount].cost}";
            UIActor.Instance.companionSoldierNameText.text = $"Retired Soldier";
            UIActor.Instance.companionSoldierDamageText.text = $"35";
            UIActor.Instance.companionSoldierHealthText.text = $"125";
            UIActor.Instance.companionSoldierStorageText.text = $"{UpgradeLists[companionCount].level * 5 + UpgradeLists[companionCount].process}";
        }

        if (UpgradeLists[weaponCount].level < WeaponManager.Instance.weapons.Count)
        {
            UIActor.Instance.maxedGunPanel.SetActive(false);
            UIActor.Instance.normalGunPanel.SetActive(true);

            UIActor.Instance.wpUpgradeCostText.text = $"{UpgradeLists[weaponCount].cost}";
        }
        else
        {
            UIActor.Instance.normalGunPanel.SetActive(false);
            UIActor.Instance.maxedGunPanel.SetActive(true);
        }
    }

    #region WEAPON UPGRADE
    public void WeaponUpgrade()
    {
        int count = Control("Weapon");
        Player player = GameObject.FindObjectOfType<Player>();

        if (UpgradeLists[count].level >= WeaponManager.Instance.weapons.Count)
            return;

        if (UpgradeLists[count].open)
        {
            if (player.myItems.money >= UpgradeLists[count].cost)
            {
                if(LevelManager.Instance.levelCount * 3 >= UpgradeLists[count].level + 1)
                {
                    player.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(false);
                    UpgradeLists[count].process++;
                    if (UpgradeLists[count].process >= 5)
                    {
                        UpgradeLists[count].level++;
                        UpgradeLists[count].process = 0;

                        UIActor.Instance.gun1.sprite = Resources.Load<Sprite>($"Gun {UpgradeLists[weaponCount].level - (((UpgradeLists[weaponCount].level - 1) / 10) * 10)}");
                        Weapon weapon = WeaponManager.Instance.weapons[UpgradeLists[weaponCount].level - 1];

                        UIActor.Instance.gun1DamageText.text = $"{(int)weapon.damage.x} - {(int)weapon.damage.y}";
                    }
                    player.GetComponent<Animator>().SetInteger("WeaponType_int", UpgradeLists[count].level - (((UpgradeLists[count].level - 1) / 10) * 10));
                    player.myItems.money -= UpgradeLists[count].cost;
                    player.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(true);
                    player.myWeapon = WeaponManager.Instance.weapons[UpgradeLists[count].level - 1];
                    player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
                    UpgradeLists[count].cost += (UpgradeLists[count].level * 12);
                    WeaponManager.Instance.Control();
                }
            }
        }
    }
    public void WeaponUpgradeRW()
    {
        int count = Control("Weapon");
        Player player = GameObject.FindObjectOfType<Player>();

        if (UpgradeLists[count].level >= WeaponManager.Instance.weapons.Count)
            return;

        if (UpgradeLists[count].open && LevelManager.Instance.levelCount * 3 >= UpgradeLists[count].level + 1)
        {
            player.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(false);
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                UIActor.Instance.gun1.sprite = Resources.Load<Sprite>($"Gun {UpgradeLists[weaponCount].level - (((UpgradeLists[weaponCount].level - 1) / 10) * 10)}");
                Weapon weapon = WeaponManager.Instance.weapons[UpgradeLists[weaponCount].level - 1];

                UIActor.Instance.gun1DamageText.text = $"{(int)weapon.damage.x} - {(int)weapon.damage.y}";
            }
            player.GetComponent<Animator>().SetInteger("WeaponType_int", UpgradeLists[count].level - (((UpgradeLists[count].level - 1) / 10) * 10));
            player.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(true);
            player.myWeapon = WeaponManager.Instance.weapons[UpgradeLists[count].level - 1];
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
            UpgradeLists[count].cost += (UpgradeLists[count].level * 10);
            WeaponManager.Instance.Control();
        }
    }
    public void WeaponUpgradeCloseButton()
    {
        UIActor.Instance.wpUpgradePanel.SetActive(false);
        UIActor.Instance.wpUpgCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);
        GameObject.FindObjectOfType<Player>().transform.parent.DOMove(playerLastPos, 1f);
        GameObject.FindObjectOfType<Player>().wpPanel = false;
        UIActor.Instance.tiers.gameObject.SetActive(false);
        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    public void UtilityUpgradeCloseButton()
    {
        UIActor.Instance.utilityUpgradePanel.SetActive(false);
        UIActor.Instance.utilityUpgCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);
        GameObject.FindObjectOfType<Player>().transform.parent.DOMove(playerLastPos, 1f);
        GameObject.FindObjectOfType<Player>().wpPanel = false;
        UIActor.Instance.tiers.gameObject.SetActive(false);
        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    public void CharacterUpgradeCloseButton()
    {
        UIActor.Instance.characterUpgradePanel.SetActive(false);
        UIActor.Instance.charUpgCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);
        GameObject.FindObjectOfType<Player>().transform.parent.DOMove(playerLastPos, 1f);
        GameObject.FindObjectOfType<Player>().wpPanel = false;
        UIActor.Instance.tiers.gameObject.SetActive(false);
        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    public void PanelFalse()
    {
        GameObject.FindObjectOfType<Player>().panel = false;
    }
    #endregion

    #region COMPANION UPGRADE
    public void CompanionUpgrade()
    {
        int count = Control("Companion");
        Player player = GameObject.FindObjectOfType<Player>();
        AI ai = UIActor.Instance.ai.transform.GetChild(0).GetComponent<AI>();

        if (UpgradeLists[count].open)
        {
            if (player.myItems.money >= UpgradeLists[count].cost)
            {
                if (UpgradeLists[count].level > 0)
                {
                    ai.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(false);
                    UpgradeLists[count].process++;
                    if (UpgradeLists[count].process >= 5)
                    {
                        UpgradeLists[count].level++;
                        UpgradeLists[count].process = 0;
                        ai.weaponCount++;
                        ai.GetComponent<Animator>().SetInteger("WeaponType_int", ai.weaponCount);
                        ai.GetComponent<Animator>().SetBool("Idle_b", true);

                        if (UpgradeLists[count].level >= 10)
                        {
                            UIActor.Instance.companionUpgradeCostText.gameObject.SetActive(false);
                            UIActor.Instance.companionUpgradeLevelText.text = $"MAX LEVEL";
                            UIActor.Instance.companionUpgradeLevelImage.fillAmount = 1;
                            UIActor.Instance.companionButtonRW.SetActive(false);
                            UIActor.Instance.companionButton.SetActive(false);
                        }
                    }
                    ai.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(true);
                    player.myItems.money -= UpgradeLists[count].cost;
                    ai.maxStackCount = UpgradeLists[companionCount].level * 5 + UpgradeLists[companionCount].process;
                    ai.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    UpgradeLists[count].level++;
                    UIActor.Instance.ai.gameObject.SetActive(true);
                    UIActor.Instance.companionSoldierOpenImage.SetActive(false);

                    aiLastPos = ai.transform.parent.position;
                    ai.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                    player.myItems.money -= UpgradeLists[count].cost;
                    ai.transform.parent.DOMove(UIActor.Instance.companionUpgCam.transform.GetChild(0).position, 1f);
                    ai.GetComponent<AI>().compPanel = true;
                }
                UpgradeLists[count].cost += 50;
            }
        }
        UIActor.Instance.companionUpgradeLevelImage.fillAmount = UpgradeLists[count].process / 5f;
    }
    public void CompanionUpgradeRW()
    {
        int count = Control("Companion");
        Player player = GameObject.FindObjectOfType<Player>();
        AI ai = UIActor.Instance.ai.transform.GetChild(0).GetComponent<AI>();

        if (UpgradeLists[count].open)
        {
            if (UpgradeLists[count].level > 0)
            {
                ai.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(false);
                UpgradeLists[count].process++;
                if (UpgradeLists[count].process >= 5)
                {
                    UpgradeLists[count].level++;
                    UpgradeLists[count].process = 0;
                    ai.weaponCount++;
                    ai.GetComponent<Animator>().SetInteger("WeaponType_int", ai.weaponCount);
                    ai.GetComponent<Animator>().SetBool("Idle_b", true);
                }
                ai.weapons.transform.GetChild(UpgradeLists[count].level - 1).gameObject.SetActive(true);
                ai.maxStackCount = UpgradeLists[companionCount].level * 5 + UpgradeLists[companionCount].process;
                ai.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            else
            {
                UpgradeLists[count].level++;
                UIActor.Instance.ai.gameObject.SetActive(true);
                UIActor.Instance.companionSoldierOpenImage.SetActive(false);

                aiLastPos = ai.transform.parent.position;
                ai.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                ai.transform.parent.DOMove(UIActor.Instance.companionUpgCam.transform.GetChild(0).position, 1f);
                ai.GetComponent<AI>().compPanel = true;
            }
            UpgradeLists[count].cost += 50;
        }
        UIActor.Instance.companionUpgradeLevelImage.fillAmount = UpgradeLists[count].process / 5f;
    }
    public void CompanionUpgradeCloseButton()
    {
        UIActor.Instance.companionUpgradePanel.SetActive(false);
        UIActor.Instance.companionUpgCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);

        if (UpgradeLists[Control("Companion")].level > 0)
        {
            GameObject.FindObjectOfType<AI>().transform.parent.DOMove(aiLastPos, 1f);
            GameObject.FindObjectOfType<AI>().compPanel = false;
        }

        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    #endregion

    #region PET UPGRADE
    public void PetUpgrade()
    {
        int count = Control("Pet");
        Player player = GameObject.FindObjectOfType<Player>();
        AnimalAI ai = UIActor.Instance.dog.GetComponent<AnimalAI>();

        if (UpgradeLists[count].open)
        {
            if (player.myItems.money >= UpgradeLists[count].cost)
            {
                UIActor.Instance.petBuyButton.interactable = false;
                UIActor.Instance.petBuyButton.GetComponent<Image>().enabled = false;
                UIActor.Instance.petBuyButton.transform.GetChild(0).gameObject.SetActive(false);
                UIActor.Instance.petBuyButton.transform.GetChild(1).gameObject.SetActive(false);
                UIActor.Instance.petBuyButton.transform.GetChild(2).gameObject.SetActive(false);
                UIActor.Instance.petBuyButton.transform.GetChild(3).gameObject.SetActive(true);
                player.myItems.money -= UpgradeLists[count].cost;
                UpgradeLists[count].level++;
                UIActor.Instance.dog.gameObject.SetActive(true);
                aiLastPos = ai.transform.position;
                ai.transform.GetComponent<Rigidbody>().isKinematic = true;
                ai.transform.DOMove(UIActor.Instance.petUpgCam.transform.GetChild(0).position, 1f);
                ai.GetComponent<AnimalAI>().petPanel = true;
                UpgradeLists[count].cost += 50;
            }
        }
    }
    public void PetUpgradeCloseButton()
    {
        UIActor.Instance.petUpgradePanel.SetActive(false);
        UIActor.Instance.petUpgCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);

        if (UpgradeLists[Control("Pet")].level > 0)
        {
            GameObject.FindObjectOfType<AnimalAI>().transform.parent.DOMove(aiLastPos, 1f);
            GameObject.FindObjectOfType<AnimalAI>().petPanel = false;
        }

        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    #endregion

    #region HEALTH UPGRADE
    public void HealthUpgrade()
    {
        int count = Control("Health");
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                if (UpgradeLists[count].level >= 10)
                {
                    UIActor.Instance.healthUpgradeCostText.transform.parent.gameObject.SetActive(false);
                    UIActor.Instance.healthUpgradeLevelText.text = $"MAX LEVEL";
                    UIActor.Instance.healthUpgradeLevelImage.fillAmount = 1;
                }
            }

            player.maxHealth = 100 + ((UpgradeLists[count].level - 1) * 50) + (UpgradeLists[count].process * 10);
            player.health = player.maxHealth;
            player.lastHealth = player.health;
            player.myItems.money -= UpgradeLists[count].cost;
            UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
    }
    public void HealthUpgradeRW()
    {
        int count = Control("Health");
        Player player = GameObject.FindObjectOfType<Player>();

        UpgradeLists[count].process++;
        if (UpgradeLists[count].process >= 5)
        {
            UpgradeLists[count].level++;
            UpgradeLists[count].process = 0;

            if (UpgradeLists[count].level >= 10)
            {
                UIActor.Instance.healthUpgradeCostText.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.healthUpgradeLevelText.text = $"MAX LEVEL";
                UIActor.Instance.healthUpgradeLevelImage.fillAmount = 1;
            }
        }

        player.maxHealth = 100 + ((UpgradeLists[count].level - 1) * 50) + (UpgradeLists[count].process * 10);
        player.health = player.maxHealth;
        player.lastHealth = player.health;
        UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
        player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
    }
    #endregion

    #region CAPACITY UPGRADE
    public void CapacityUpgrade()
    {
        int count = Control("Capacity");
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                if (UpgradeLists[count].level >= 10)
                {
                    UIActor.Instance.capacityUpgradeCostText.transform.parent.gameObject.SetActive(false);
                    UIActor.Instance.capacityUpgradeLevelText.text = $"MAX LEVEL";
                    UIActor.Instance.capacityUpgradeLevelImage.fillAmount = 1;
                }
            }
            player.maxStackCount += UpgradeLists[count].level;
            player.myItems.money -= UpgradeLists[count].cost;
            UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
    }
    public void CapacityUpgradeRW()
    {
        int count = Control("Capacity");
        Player player = GameObject.FindObjectOfType<Player>();

        UpgradeLists[count].process++;
        if (UpgradeLists[count].process >= 5)
        {
            UpgradeLists[count].level++;
            UpgradeLists[count].process = 0;

            if (UpgradeLists[count].level >= 10)
            {
                UIActor.Instance.capacityUpgradeCostText.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.capacityUpgradeLevelText.text = $"MAX LEVEL";
                UIActor.Instance.capacityUpgradeLevelImage.fillAmount = 1;
            }
        }
        player.maxStackCount += UpgradeLists[count].level;
        UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
        player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
    }
    #endregion

    #region SPEED UPGRADE
    public void SpeedUpgrade()
    {
        int count = Control("Speed");
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                if (UpgradeLists[count].level >= 10)
                {
                    UIActor.Instance.speedUpgradeCostText.transform.parent.gameObject.SetActive(false);
                    UIActor.Instance.speedUpgradeLevelText.text = $"MAX LEVEL";
                    UIActor.Instance.speedUpgradeLevelImage.fillAmount = 1;
                }
            }
            player.transform.parent.GetComponent<JoystickPlayerExample>().speed = 10 + ((UpgradeLists[count].level - 1) * 0.6f) + (UpgradeLists[count].process * 0.12f);
            player.myItems.money -= UpgradeLists[count].cost;
            UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            UIActor.Instance.speedUpgradeValueText.text = $"+ {player.transform.parent.GetComponent<JoystickPlayerExample>().speed} SPEED";
        }
    }
    public void SpeedUpgradeRW()
    {
        int count = Control("Speed");
        Player player = GameObject.FindObjectOfType<Player>();

        UpgradeLists[count].process++;
        if (UpgradeLists[count].process >= 5)
        {
            UpgradeLists[count].level++;
            UpgradeLists[count].process = 0;

            if (UpgradeLists[count].level >= 10)
            {
                UIActor.Instance.speedUpgradeCostText.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.speedUpgradeLevelText.text = $"MAX LEVEL";
                UIActor.Instance.speedUpgradeLevelImage.fillAmount = 1;
            }
        }
        player.transform.parent.GetComponent<JoystickPlayerExample>().speed = 10 + ((UpgradeLists[count].level - 1) * 0.6f) + (UpgradeLists[count].process * 0.12f);
        UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
        player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

        UIActor.Instance.speedUpgradeValueText.text = $"+ {player.transform.parent.GetComponent<JoystickPlayerExample>().speed} SPEED";
    }
    #endregion

    #region MONEY UPGRADE
    public void MoneyUpgrade()
    {
        int count = Control("Money");
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                if (UpgradeLists[count].level >= 10)
                {
                    UIActor.Instance.moneyUpgradeCostText.transform.parent.gameObject.SetActive(false);
                    UIActor.Instance.moneyUpgradeLevelText.text = $"MAX LEVEL";
                    UIActor.Instance.moneyUpgradeLevelImage.fillAmount = 1;
                }
            }
            player.earnMoneyValue = 10 + ((UpgradeLists[count].level - 1) * 1.66f) + (UpgradeLists[count].process * 0.33f);
            player.myItems.money -= UpgradeLists[count].cost;
            UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            UIActor.Instance.moneyUpgradeValueText.text = $"+ {player.earnMoneyValue * 3f} MONEY";
        }
    }
    public void MoneyUpgradeRW()
    {
        int count = Control("Money");
        Player player = GameObject.FindObjectOfType<Player>();

        UpgradeLists[count].process++;
        if (UpgradeLists[count].process >= 5)
        {
            UpgradeLists[count].level++;
            UpgradeLists[count].process = 0;

            if (UpgradeLists[count].level >= 10)
            {
                UIActor.Instance.moneyUpgradeCostText.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.moneyUpgradeLevelText.text = $"MAX LEVEL";
                UIActor.Instance.moneyUpgradeLevelImage.fillAmount = 1;
            }
        }
        player.earnMoneyValue = 10 + ((UpgradeLists[count].level - 1) * 1.66f) + (UpgradeLists[count].process * 0.33f);
        UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
        player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

        UIActor.Instance.moneyUpgradeValueText.text = $"+ {player.earnMoneyValue * 3f} MONEY";
    }
    #endregion

    #region ARMOR UPGRADE
    public void ArmorUpgrade()
    {
        int count = Control("Armor");
        Player player = GameObject.FindObjectOfType<Player>();
        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            UpgradeLists[count].process++;
            if (UpgradeLists[count].process >= 5)
            {
                UpgradeLists[count].level++;
                UpgradeLists[count].process = 0;

                if (UpgradeLists[count].level >= 10)
                {
                    UIActor.Instance.armorUpgradeCostText.transform.parent.gameObject.SetActive(false);
                    UIActor.Instance.armorUpgradeLevelText.text = $"MAX LEVEL";
                    UIActor.Instance.armorUpgradeLevelImage.fillAmount = 1;
                }
            }
            player.armor = 10 + ((UpgradeLists[count].level - 1) * 5) + (UpgradeLists[count].process * 1);
            player.myItems.money -= UpgradeLists[count].cost;
            UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
            player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

            UIActor.Instance.armorUpgradeValueText.text = $"+ {player.armor} ARMOR";
        }
    }
    public void ArmorUpgradeRW()
    {
        int count = Control("Armor");
        Player player = GameObject.FindObjectOfType<Player>();

        UpgradeLists[count].process++;
        if (UpgradeLists[count].process >= 5)
        {
            UpgradeLists[count].level++;
            UpgradeLists[count].process = 0;

            if (UpgradeLists[count].level >= 10)
            {
                UIActor.Instance.armorUpgradeCostText.transform.parent.gameObject.SetActive(false);
                UIActor.Instance.armorUpgradeLevelText.text = $"MAX LEVEL";
                UIActor.Instance.armorUpgradeLevelImage.fillAmount = 1;
            }
        }
        player.armor = 10 + ((UpgradeLists[count].level - 1) * 5) + (UpgradeLists[count].process * 1);
        UpgradeLists[count].cost += ((UpgradeLists[count].level) * 10);
        player.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();

        UIActor.Instance.armorUpgradeValueText.text = $"+ {player.armor} ARMOR";
    }
    #endregion

    #region DOCTOR
    public void DoctorMoneyButton()
    {
        int count = Control("Doctor");
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= UpgradeLists[count].cost)
        {
            player.myItems.money -= UpgradeLists[count].cost;
            StartCoroutine(DoctorRestore());
        }
    }
    public IEnumerator DoctorRestore()
    {
        int count = Control("Doctor");
        Player player = GameObject.FindObjectOfType<Player>();

        while (player.health < player.maxHealth)
        {
            player.health++;
            UIActor.Instance.doctorHealthText.text = $"%{(int)((player.health / player.maxHealth) * 100)}";
            UIActor.Instance.doctorCostText.text = $"{(int)(((player.maxHealth - player.health) / UpgradeLists[count].cost) * UpgradeLists[count].cost)}";
            yield return new WaitForSeconds(0.01f);
        }
        player.lastHealth = player.health;

        UIActor.Instance.doctorRestoreMoneyButton.interactable = false;
    }
    public IEnumerator DoctorRewardedRestore()
    {
        int count = Control("Doctor");
        Player player = GameObject.FindObjectOfType<Player>();

        while (player.health < player.maxHealth)
        {
            player.health++;
            UIActor.Instance.doctorHealthText.text = $"%{(int)((player.health / player.maxHealth) * 100)}";
            UIActor.Instance.doctorCostText.text = $"{0}";
            yield return new WaitForSeconds(0.01f);
        }
        player.lastHealth = player.health;

        UIActor.Instance.doctorRestoreMoneyButton.interactable = false;
    }
    public void DoctorCloseButton()
    {
        UIActor.Instance.doctorPanel.SetActive(false);
        UIActor.Instance.doctorCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);
        Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
    #endregion
    public int Control(string name)
    {
        for (int i = 0; i < UpgradeLists.Count; i++)
        {
            if (UpgradeLists[i].name == name)
            {
                return i;
            }
        }
        return 0;
    }
}
