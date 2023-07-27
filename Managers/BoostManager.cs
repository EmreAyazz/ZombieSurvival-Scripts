using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
    public static BoostManager Instance;
     
    public List<Boost> boost;
    [System.Serializable]
    public class Boost
    {
        public string boostName;
        public int level;
        public ZombieManager area;
        public bool okay;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Boost 0"))
        {
            Load();
        }
    }

    public void Save()
    {
        for (int i = 0; i < boost.Count; i++)
        {
            PlayerPrefs.SetString($"Boost {i}", boost[i].okay.ToString());
        }
    }
    public void Load()
    {
        for (int i = 0; i < boost.Count; i++)
        {
            if (PlayerPrefs.GetString($"Boost {i}") == "True")
            {
                boost[i].okay = true;
                UIActor.Instance.mainPanel.transform.Find(boost[i].boostName).GetComponent<Image>().enabled = true;
                UIActor.Instance.mainPanel.transform.Find(boost[i].boostName).GetComponent<Button>().enabled = true;
            }
            else
            {
                boost[i].okay = false;
            }
        }
    }
}
