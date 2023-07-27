using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CarControl : MonoBehaviour
{
    public float speed;

    private float lastPos, currentPos;
    private float lastMousePos, currentMousePos;
    float xPos;

    public List<GameObject> ways;
    public GameObject way, finishWay;

    public GameObject mainCam;

    public bool carkifelekB, carkOkay, verildi;
    public float carkifelekSpeed, randomSpeed;

    bool start;

    bool finish;


    // Start is called before the first frame update
    void Start()
    {
        xPos = 532;

        transform.GetChild(Random.Range(0, 4)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.Instance.start)
            return;

        float distance = Vector3.Distance(transform.position, Vector3.zero);

        if (finish && speed > 0)
            speed -= Time.deltaTime * 45f;
        if (speed <= 0)
        {
            speed = 0;
        }

        if (ways.Count < 3)
        {
            if ((int)(distance / 149.5f) > ways.Count)
            {
                GameObject newWay = Instantiate(way, new Vector3(520.78f, 1.86f, way.transform.position.z + (149.5f * (ways.Count + 2))), way.transform.rotation);
                newWay.SetActive(true);
                newWay.transform.SetParent(LevelManager.Instance.bonusLevel.transform);
                ways.Add(newWay);
            }
        }
        else
        {
            if (ways.Count < 4)
            {
                GameObject newWay = Instantiate(finishWay, new Vector3(520.78f, 1.86f, way.transform.position.z + (149.5f * (ways.Count + 2))), way.transform.rotation);
                newWay.SetActive(true);
                newWay.transform.SetParent(LevelManager.Instance.bonusLevel.transform);
                ways.Add(newWay);
            }
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (carkifelekB)
        {
            GameObject carkifelek = GameObject.FindGameObjectWithTag("Carkifelek").gameObject;
            carkifelek.transform.GetChild(0).Rotate(Vector3.forward * Time.deltaTime * carkifelekSpeed);

            Player player = GameObject.FindObjectOfType<Player>();

            if (!carkOkay)
                carkifelekSpeed += Time.deltaTime * (randomSpeed / 2f);
            else
            {
                if (carkifelekSpeed > 0)
                    carkifelekSpeed -= Time.deltaTime * (randomSpeed / 4f);
                else
                {
                    carkifelekSpeed = 0;
                    if (!verildi)
                    {
                        UIActor.Instance.spinWinPanel.SetActive(true);

                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 0 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 45)
                        {
                            player.myItems.money += 500;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.money;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "$500";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 45 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 90)
                        {
                            player.myItems.money += 5000;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.money;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "$5000";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 90 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 135)
                        {
                            AdManager.Instance.damage++;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.damageBoost;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "1x Damage Boost";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 135 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 180)
                        {
                            player.myItems.money += 10000;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.money;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "$10000";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 180 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 225)
                        {
                            AdManager.Instance.storage++;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.capacityBoost;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "1x Capacity Boost";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 225 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 270)
                        {
                            AdManager.Instance.speed++;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.headBoost;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "1x Head Boost";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 270 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 315)
                        {
                            player.myItems.money += 2500;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.money;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "$2500";
                        }
                        if (carkifelek.transform.GetChild(0).localEulerAngles.z > 315 && carkifelek.transform.GetChild(0).localEulerAngles.z <= 359.9f)
                        {
                            AdManager.Instance.home++;
                            UIActor.Instance.spinImage.GetComponent<Image>().sprite = UIActor.Instance.homeBoost;
                            UIActor.Instance.spinText.GetComponent<TextMeshProUGUI>().text = "1x Home Boost";
                        }

                        verildi = true;
                    }
                }
            }


            if (carkifelekSpeed >= randomSpeed)
                carkOkay = true;
        }

        if (!finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPos = transform.position.x;
                lastMousePos = Input.mousePosition.x;
                transform.DOKill();
            }
            if (Input.GetMouseButton(0))
            {
                currentMousePos = Input.mousePosition.x;

                xPos = Mathf.Clamp(lastPos + ((currentMousePos - lastMousePos) / 100f), 526, 536);

                transform.DOMoveX(xPos, 0.5f);
            }
        }
        transform.DOLookAt(new Vector3(xPos, transform.position.y, transform.position.z + 5), 0.3f);
    }

    public void Next()
    {
        Destroy(LevelManager.Instance.bonusLevel);
        UIActor.Instance.spinWinPanel.SetActive(false);
        UIActor.Instance.mainCam.SetActive(true);
        UIActor.Instance.joystick.SetActive(true);
        UIActor.Instance.healthbar.transform.parent.gameObject.SetActive(true);
        UIActor.Instance.cooldownbar.transform.parent.gameObject.SetActive(true);
        UIActor.Instance.baseMap.SetActive(true);
        LevelManager.Instance.lastLevel.SetActive(true);

        int level = 0;
        if (LevelManager.Instance.levelCount % 5 != 0)
        {
            level = LevelManager.Instance.levelCount - (5 * (LevelManager.Instance.levelCount / 5)) - 1;
        }
        else
        {
            level = LevelManager.Instance.levelCount - (5 * ((LevelManager.Instance.levelCount / 5) - 1)) - 1;
        }
        RenderSettings.ambientSkyColor = LevelManager.Instance.skyboxies[level];
        RenderSettings.fogColor = LevelManager.Instance.fog[level];
    }
    public void SpinRW()
    {
        Destroy(LevelManager.Instance.bonusLevel);
        UIActor.Instance.spinWinPanel.SetActive(false);
        UIActor.Instance.mainCam.SetActive(true);
        UIActor.Instance.joystick.SetActive(true);
        UIActor.Instance.healthbar.transform.parent.gameObject.SetActive(true);
        UIActor.Instance.cooldownbar.transform.parent.gameObject.SetActive(true);
        UIActor.Instance.baseMap.SetActive(true);
        LevelManager.Instance.lastLevel.SetActive(true);

        int level = 0;
        if (LevelManager.Instance.levelCount % 5 != 0)
        {
            level = LevelManager.Instance.levelCount - (5 * (LevelManager.Instance.levelCount / 5)) - 1;
        }
        else
        {
            level = LevelManager.Instance.levelCount - (5 * ((LevelManager.Instance.levelCount / 5) - 1)) - 1;
        }
        RenderSettings.ambientSkyColor = LevelManager.Instance.skyboxies[level];
        RenderSettings.fogColor = LevelManager.Instance.fog[level];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            finish = true;

            GameObject carkifelek = GameObject.FindGameObjectWithTag("Carkifelek").gameObject;

            mainCam.GetComponent<Cam>().enabled = false;
            mainCam.transform.DOMove(carkifelek.transform.GetChild(1).transform.position, 2f);
            mainCam.transform.DORotate(carkifelek.transform.GetChild(1).transform.eulerAngles, 2f).OnComplete(() =>
            {
                UIActor.Instance.spinButton.SetActive(true);
            });
        }
    }
}

