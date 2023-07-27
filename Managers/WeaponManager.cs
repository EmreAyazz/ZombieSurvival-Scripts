using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TT.Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public List<Weapon> weapons;

    public List<GameObject> weaponPanels;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke("Initialize", 0.5f);
    }

    public void Initialize()
    {
        int a = 0;
        int levelC = 1;
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].level = levelC;

            if (a < 2)
                a++;
            else
            {
                a = 0;
                levelC++;
            }
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject newWeapon = Instantiate(UIActor.Instance.gunPanel, UIActor.Instance.weaponsPanel.transform);
            weaponPanels.Add(newWeapon);
            newWeapon.SetActive(true);

            GunPanel gun = newWeapon.GetComponent<GunPanel>();
            gun.levelText.text = "Level 1";
            gun.nameText.text = weapons[i].name;
            gun.damageText.text = $"{(int)weapons[i].damage.x} - {(int)weapons[i].damage.y}";
            gun.gunImage.sprite = Resources.Load<Sprite>($"Gun {(i % 10) + 1}");
            gun.level = weapons[i].level;

            gun.count = i;
            switch (gun.count / 10)
            {
                case 0:
                    gun.tier.color = new Color(0.75f, 0.75f, 0.75f, 1);
                    break;
                case 1:
                    gun.tier.color = new Color(0, 0.75f, 1, 1);
                    break;
                case 2:
                    gun.tier.color = new Color(0.5f, 0.15f, 1, 1);
                    break;
                case 3:
                    gun.tier.color = new Color(0.96f, 0.75f, 0.1f, 1);
                    break;
            }

            if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level > gun.count + 1)
            {
                gun.progressBar.fillAmount = 1;
            }
            else if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level == gun.count + 1)
            {
                gun.progressBar.fillAmount = UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].process * 0.2f;
            }
            else
            {
                gun.progressBar.fillAmount = 0;
            }
        }

        Control();
    }

    public void Control()
    {
        for (int i = 0; i < weaponPanels.Count; i++)
        {
            GunPanel gun = weaponPanels[i].GetComponent<GunPanel>();

            if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level > gun.count + 1)
            {
                gun.progressBar.fillAmount = 1;
            }
            else if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level == gun.count + 1)
            {
                gun.progressBar.fillAmount = UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].process * 0.2f;
                gun.Select();
            }
            else
            {
                gun.progressBar.fillAmount = 0;
                gun.selectButton.interactable = false;
                gun.selectButton.GetComponent<Image>().enabled = false;
                gun.selectButton.transform.GetChild(0).GetComponent<Text>().text = "Locked";
            }
        }
        UIActor.Instance.weaponsPanel.transform.localPosition = new Vector3(UIActor.Instance.weaponsPanel.transform.localPosition.x, (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level - 1) * 265f, UIActor.Instance.weaponsPanel.transform.localPosition.z);
    }
}
