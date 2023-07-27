using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public int value;
    public int currentValue;

    public Image current, next, back;

    public List<int> mySkins;
    public Button select, buy, rewarded;

    private void Awake()
    {
        Instance = this;

        Load();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!mySkins.Contains(1))
            mySkins.Add(1);

        current.sprite = Resources.Load<Sprite>($"Character{value}");
        next.sprite = value < 12 ? Resources.Load<Sprite>($"Character{value + 1}") : Resources.Load<Sprite>($"Character{1}");
        back.sprite = value > 1 ? Resources.Load<Sprite>($"Character{value - 1}") : Resources.Load<Sprite>($"Character{12}");

        if (value == currentValue)
        {
            select.interactable = false;
            select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
        }
        else
        {
            select.interactable = true;
            select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select";
        }

        for (int i = 0; i < mySkins.Count; i++)
        {
            if (value == mySkins[i])
            {
                select.gameObject.SetActive(true);
                buy.gameObject.SetActive(false);
                rewarded.gameObject.SetActive(false);
                break;
            }
            select.gameObject.SetActive(false);
            if (value <= 9)
            {
                buy.gameObject.SetActive(true);
                rewarded.gameObject.SetActive(false);
            }
            else
            {
                buy.gameObject.SetActive(false);
                rewarded.gameObject.SetActive(true);
            }
        }
    }

    public void Change(int direction)
    {
        value += direction;

        if (value > 12)
            value = 1;
        if (value < 1)
            value = 12;

        current.sprite = Resources.Load<Sprite>($"Character{value}");
        next.sprite = value < 12 ? Resources.Load<Sprite>($"Character{value + 1}") : Resources.Load<Sprite>($"Character{1}");
        back.sprite = value > 1 ? Resources.Load<Sprite>($"Character{value - 1}") : Resources.Load<Sprite>($"Character{12}");

        if (value == currentValue)
        {
            select.interactable = false;
            select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
        }
        else
        {
            select.interactable = true;
            select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select";
        }

        for (int i = 0; i < mySkins.Count; i++)
        {
            if (value == mySkins[i])
            {
                select.gameObject.SetActive(true);
                buy.gameObject.SetActive(false);
                rewarded.gameObject.SetActive(false);
                break;
            }
            select.gameObject.SetActive(false);
            if (value <= 9)
            {
                buy.gameObject.SetActive(true);
                rewarded.gameObject.SetActive(false);
            }
            else
            {
                buy.gameObject.SetActive(false);
                rewarded.gameObject.SetActive(true);
            }
        }
    }

    public void Buy(int money)
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (player.myItems.money >= money)
        {
            player.transform.GetChild(currentValue).gameObject.SetActive(false);
            player.transform.GetChild(value).gameObject.SetActive(true);
            currentValue = value;

            mySkins.Add(value);

            player.myItems.money -= money;

            select.gameObject.SetActive(true);
            select.interactable = false;
            select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";
            buy.gameObject.SetActive(false);
        }

        Save();
    }

    public void Select()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        player.transform.GetChild(currentValue).gameObject.SetActive(false);
        player.transform.GetChild(value).gameObject.SetActive(true);
        currentValue = value;
        select.interactable = false;
        select.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Selected";

        Save();
    }

    public void Save()
    {
        for (int i = 0; i < mySkins.Count; i++)
        {
            PlayerPrefs.SetInt($"Character{mySkins[i]}", mySkins[i]);
        }

        PlayerPrefs.SetInt("CurrentCharacter", currentValue);
    }
    public void Load()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        for (int i = 1; i <= 12; i++)
        {
            if (PlayerPrefs.HasKey($"Character{i}"))
            {
                mySkins.Add(i);
            }
        }

        if (PlayerPrefs.HasKey("CurrentCharacter"))
            currentValue = PlayerPrefs.GetInt("CurrentCharacter");

        value = currentValue;

        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(value).gameObject.SetActive(true);
    }
}
