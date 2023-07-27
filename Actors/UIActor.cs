using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIActor : MonoBehaviour
{
    public static UIActor Instance;

    [Header("PlayerUI")]
    public Image healthbar;
    public Image cooldownbar;
    public Image timeBar;
    public Image maxStackBar;
    public GameObject joystick;
    public GameObject maxStackText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI headsText;
    public Image lockImage;
    public GameObject keyPanel;
    public Image keys;

    public Image deadPanelTimer;
    public TextMeshProUGUI deadPanelTimerText;
    public Button reviveButton;
    public TextMeshProUGUI hintPanel;

    public TextMeshPro levelPanelText;

    public GameObject inGamePanel;
    public GameObject deadPanel;
    public GameObject mainPanel;
    public GameObject joystickPanel;
    public GameObject normalGunPanel, maxedGunPanel;
    public GameObject weaponsPanel, gunPanel, weaponScrollView;
    public GameObject spinWinPanel, spinImage, spinText;
    public Image levelGecis;

    public RawImage tiers;

    public Sprite money, damageBoost, headBoost, homeBoost, capacityBoost;

    [Header("Players")]
    public GameObject ai;
    public GameObject dog;
    public GameObject info;

    [Header("Base")]
    public GameObject baseMap;
    public GameObject npcArea;
    public GameObject enemyArea;
    public GameObject center;
    public GameObject exchange3xArea;
    public GameObject exchange3xPanel;
    public GameObject trader;
    public GameObject traderPanel;
    public GameObject bonuslevel;
    public GameObject spinButton;
    public GameObject swipeScreen;
    public GameObject portal;

    public GameObject bossPanel;
    public Image boss1Image, boss2Image;
    public List<Image> gunImages;

    [Header("Camera")]
    public GameObject mainCam;
    public GameObject playerCam;
    public GameObject baseCam;
    public GameObject baseUpgCam;
    public GameObject wpUpgCam;
    public GameObject utilityUpgCam;
    public GameObject charUpgCam;
    public GameObject petUpgCam;
    public GameObject towerUpgCam;
    public GameObject companionUpgCam;
    public GameObject doctorCam;
    public GameObject magicCam;

    [Header("Particles")]
    public GameObject rocketExplosion;

    [Header("Upgrade Panels")]

    public GameObject characterUpgradePanel;
    public GameObject utilityUpgradePanel;
    public GameObject wpUpgradePanel;
    public TextMeshProUGUI wpUpgradeCostText;
    public TextMeshProUGUI wpUpgradeLevelText;
    public Image wpUpgradeLevelImage;
    public GameObject gunUpgradePanel;
    public GameObject skillsPanel;

    [Space(10f)]

    public GameObject petUpgradePanel;
    public TextMeshProUGUI petUpgradeCostText;
    public TextMeshProUGUI petUpgradeLevelText;
    public Button petBuyButton;

    [Space(10f)]

    public GameObject companionUpgradePanel;
    public TextMeshProUGUI companionUpgradeCostText;
    public TextMeshProUGUI companionUpgradeLevelText;
    public Image companionUpgradeLevelImage;
    public Text companionSoldierNameText;
    public Text companionSoldierDamageText;
    public Text companionSoldierHealthText;
    public Text companionSoldierStorageText;
    public GameObject companionSoldierOpenImage;
    public GameObject companionButtonRW;
    public GameObject companionButton;
    public GameObject companionBuyButton;

    [Space(10f)]

    public GameObject wpButton;
    public Image gun1, gun2;
    public TextMeshProUGUI gun1DamageText, gun2DamageText;
    public GameObject wpButtonRW;

    [Space(10f)]

    public TextMeshProUGUI healthUpgradeCostText;
    public TextMeshProUGUI healthUpgradeLevelText;
    public TextMeshProUGUI healthUpgradeValueText;
    public GameObject healthButtonRW;
    public GameObject healthButton;
    public Image healthUpgradeLevelImage;

    [Space(10f)]

    public TextMeshProUGUI speedUpgradeCostText;
    public TextMeshProUGUI speedUpgradeLevelText;
    public TextMeshProUGUI speedUpgradeValueText;
    public GameObject speedButtonRW;
    public GameObject speedButton;
    public Image speedUpgradeLevelImage;

    [Space(10f)]

    public TextMeshProUGUI capacityUpgradeCostText;
    public TextMeshProUGUI capacityUpgradeLevelText;
    public TextMeshProUGUI capacityUpgradeValueText;
    public GameObject capacityButtonRW;
    public GameObject capacityButton;
    public Image capacityUpgradeLevelImage;

    [Space(10f)]

    public Text moneyUpgradeCostText;
    public Text moneyUpgradeLevelText;
    public Text moneyUpgradeValueText;
    public GameObject moneyButtonRW;
    public GameObject moneyButton;
    public Image moneyUpgradeLevelImage;

    [Space(10f)]

    public TextMeshProUGUI armorUpgradeCostText;
    public TextMeshProUGUI armorUpgradeLevelText;
    public TextMeshProUGUI armorUpgradeValueText;
    public GameObject armorButtonRW;
    public GameObject armorButton;
    public Image armorUpgradeLevelImage;

    [Space(10f)]

    public GameObject doctorPanel;
    public TextMeshProUGUI doctorCostText;
    public TextMeshProUGUI doctorHealthText;
    public Button doctorRestoreMoneyButton;

    private void Awake()
    {
        Instance = this;
    }
}
