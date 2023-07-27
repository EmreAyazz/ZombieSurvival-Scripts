using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TT.Weapon;

public class GunPanel : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI levelText, nameText, damageText;
    public Image gunImage, progressBar;
    public Button selectButton;
    public Image tier;
    public int level;

    [Space(10)]
    public Material tier2;
    public Material tier3, tier4;
    public RawImage tiers;

    public void Select()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        for (int i = 0; i < player.weapons.transform.childCount; i++)
        {
            player.weapons.transform.GetChild(i).gameObject.SetActive(false);
        }
        player.weapons.transform.GetChild(count).gameObject.SetActive(true);

        for (int i = 0; i < WeaponManager.Instance.weaponPanels.Count; i++)
        {
            if (i + 1 <= UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control("Weapon")].level)
            {
                WeaponManager.Instance.weaponPanels[i].GetComponent<GunPanel>().selectButton.interactable = true;
                WeaponManager.Instance.weaponPanels[i].GetComponent<GunPanel>().selectButton.GetComponent<Image>().enabled = true;
                WeaponManager.Instance.weaponPanels[i].GetComponent<GunPanel>().selectButton.transform.GetChild(0).GetComponent<Text>().text = "Select";
                WeaponManager.Instance.weaponPanels[i].GetComponent<Image>().color = new Color(0, 0, 0, 0.3f);
            }
        }
        WeaponManager.Instance.weaponPanels[count].GetComponent<GunPanel>().selectButton.interactable = false;
        WeaponManager.Instance.weaponPanels[count].GetComponent<GunPanel>().selectButton.GetComponent<Image>().enabled = false;
        WeaponManager.Instance.weaponPanels[count].GetComponent<GunPanel>().selectButton.transform.GetChild(0).GetComponent<Text>().text = "Selected";
        WeaponManager.Instance.weaponPanels[count].GetComponent<Image>().color = new Color(0, 1, 0, 0.7f);

        switch (count / 10)
        {
            case 0:
                tiers.material = null;
                tiers.color = new Color(1, 1, 1, 1);
                break;
            case 1:
                tiers.material = tier2;
                tiers.color = new Color(1, 1, 1, 1);
                break;
            case 2:
                tiers.material = tier3;
                tiers.color = new Color(1, 1, 1, 1);
                break;
            case 3:
                tiers.material = tier4;
                tiers.color = new Color(1, 1, 1, 1);
                break;
        }

        player.GetComponent<Animator>().SetInteger("WeaponType_int", (count % 10) + 1);
        player.myWeapon = WeaponManager.Instance.weapons[count];
        UIActor.Instance.gun1.sprite = Resources.Load<Sprite>($"Gun {(count % 10) + 1}");

        UIActor.Instance.gun1DamageText.text = $"{(int)WeaponManager.Instance.weapons[count].damage.x} - {(int)WeaponManager.Instance.weapons[count].damage.y}";

    }
}
