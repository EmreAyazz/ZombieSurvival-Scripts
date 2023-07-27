using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance;

    public List<Skill> skills;

    public List<GameObject> skillsUI;

    public List<GameObject> mySkills;

    public List<GameObject> mySkillsCancelButtons;

    public List<GameObject> panels;

    Player player;
    public GameObject warn;

    [System.Serializable]
    public class Skill
    {
        public string name;
        public Sprite image;
        public int level;
        public int process;
        public float damage;
        public float cost;
        public bool open;
        public bool selected;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    public void Control()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (!skills[i].open)
            {
                skillsUI[i].transform.GetChild(7).gameObject.SetActive(false);
                skillsUI[i].transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                skillsUI[i].transform.GetChild(4).GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                skillsUI[i].transform.GetChild(7).gameObject.SetActive(true);
                skillsUI[i].transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                skillsUI[i].transform.GetChild(4).GetChild(3).gameObject.SetActive(false);
            }

            if (skills[i].process >= 5)
            {
                skills[i].process = 0;
                skills[i].level += 1;
            }

            if (player.myItems.money <= skills[i].cost)
            {
                skillsUI[i].transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                skillsUI[i].transform.GetChild(6).gameObject.SetActive(false);
            }

            if (!skills[i].selected)
            {
                skillsUI[i].transform.GetChild(7).GetComponent<Button>().interactable = true;
            }

            skillsUI[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"DAMAGE: +{skills[i].damage}";
            skillsUI[i].transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = skills[i].process / 5f;
            skillsUI[i].transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{(int)skills[i].cost}";
            skillsUI[i].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = $"LEVEL {skills[i].level}";
        }
        for (int i = 0; i < mySkills.Count; i++)
        {
            if (mySkills[i].transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                mySkillsCancelButtons[i].SetActive(false);
            }
            else
            {
                mySkillsCancelButtons[i].SetActive(true);
            }
        }
    }

    public void Upgrade(int value)
    {
        if (skills[value].level == 0)
        {
            player.myItems.money -= (int)skills[value].cost;
            skills[value].level += 1;
            skills[value].cost += (skills[value].level * 10);
            skills[value].open = true;
            skills[value].damage += 20;
        }
        else
        {
            player.myItems.money -= (int)skills[value].cost;
            skills[value].process += 1;
            skills[value].cost += (skills[value].level * 10);
            skills[value].damage += 5;
        }
        Control();
        if (!skills[value].selected)
        {
            for (int i = 0; i < mySkills.Count; i++)
            {
                if (mySkills[i].transform.GetChild(0).GetComponent<Image>().sprite == null)
                {
                    Select(value);
                    return;
                }
            }
        }
    }

    public void Select(int value)
    {
        player = GameObject.FindObjectOfType<Player>();
        for (int i = 0; i < mySkills.Count; i++)
        {
            if (mySkills[i].transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                mySkills[i].transform.GetChild(0).GetComponent<Image>().sprite = skills[value].image;
                player.skills[i] = value;
                skillsUI[value].transform.GetChild(7).GetComponent<Button>().interactable = false;
                skills[value].selected = true;
                mySkillsCancelButtons[i].SetActive(true);
                return;
            }
        }
        GameObject newWarn = Instantiate(warn, UIActor.Instance.skillsPanel.transform);
        newWarn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ALREADY HAVE 3 SKILLS";
        newWarn.SetActive(true);
    }

    public void Close(int value)
    {
        mySkills[value].transform.GetChild(0).GetComponent<Image>().sprite = null;
        skills[player.skills[value]].selected = false;
        player.skills[value] = 10;

        for (int i = 0; i < skillsUI.Count; i++)
        {
            if (!skills[i].selected)
            {
                skillsUI[i].transform.GetChild(7).GetComponent<Button>().interactable = true;
            }
        }

        Control();
    }

    public void SelectPanel(int value)
    {
        panels[value].GetComponent<Image>().color = new Color(0.454902f, 0.4941176f, 0.8666667f);

        for (int i = 0; i < panels.Count; i++)
        {
            if (i != value)
            {
                panels[i].GetComponent<Image>().color = new Color(0.5758633f, 0.6072725f, 0.9056604f);
            }
        }
    }
    public void MagicCloseButton()
    {
        UIActor.Instance.skillsPanel.SetActive(false);
        UIActor.Instance.magicCam.SetActive(false);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.joystickPanel.SetActive(true);
        UpgradeManager.Instance.Invoke("PanelFalse", 2f);
        Time.timeScale = 1;
    }
}
