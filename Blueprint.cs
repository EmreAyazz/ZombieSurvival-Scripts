using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Blueprint : MonoBehaviour
{
    public BlueprintType blueprintType;

    string mainText;
    float mainFontSize;

    Player player;

    bool completed;

    [System.Serializable]
    public class BlueprintType
    {
        public string name;
        public bool open;
        public bool canUnlock;

        public int requiredBaseLevel;

        public List<Required> required;

        [System.Serializable]
        public class Required
        {
            public string name;
            public int count;
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();

        mainText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text;
        mainFontSize = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().fontSize;

        if (PlayerPrefs.HasKey($"{blueprintType.name}Complete"))
        {
            if (PlayerPrefs.GetString($"{blueprintType.name}Complete") == "True")
            {
                completed = true;
            }
        }
        Control();
    }

    private void Update()
    {
        Control();
    }

    public void Control()
    {
        if (UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control(blueprintType.name)].open)
        {
            blueprintType.required[0].count = 0;
        }

        TextMeshPro text = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();

        if (LevelManager.Instance.levelCount < blueprintType.requiredBaseLevel)
        {
            text.fontSize = 16.2f;
            text.text = $"DEFEAT LEVEL {blueprintType.requiredBaseLevel - 1} BOSS TO UNLOCK";
            return;
        }
        blueprintType.canUnlock = true;

        GameObject moneyImage = null;

        if (blueprintType.required[0].name != "Head")
            moneyImage = transform.GetChild(0).GetChild(1).gameObject;
        else
            moneyImage = transform.GetChild(0).GetChild(2).gameObject;

        if (isCompleted())
        {
            UpgradeManager.Instance.UpgradeLists[UpgradeManager.Instance.Control(blueprintType.name)].open = true;
            blueprintType.open = true;
            text.text = mainText;
            text.fontSize = mainFontSize;
            moneyImage.SetActive(false);

            if (!completed)
            {
                completed = true;
                PlayerPrefs.SetString($"{blueprintType.name}Complete", completed.ToString());
                TutorialManager.Instance.queue.Add(gameObject);
            }
        }
        else
        {
            text.fontSize = 19.2f;

            if (blueprintType.required[0].name != "Head")
                text.text = "UNLOCK\n " + blueprintType.required[0].count;
            else
                text.text = "UNLOCK\n " + (blueprintType.required[0].count - player.myItems.head);

            moneyImage.SetActive(true);
        }
    }

    public bool isCompleted()
    {
        if(blueprintType.required[0].name != "Head")
        {
            for (int i = 0; i < blueprintType.required.Count; i++)
            {
                if (blueprintType.required[i].count > 0)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            if (blueprintType.required[0].count > player.myItems.head)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}
