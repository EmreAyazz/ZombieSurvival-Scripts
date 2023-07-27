using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseUpgrade : MonoBehaviour
{
    public int upgradeLevel;
    public bool upgrading;

    // Start is called before the first frame update
    void Start()
    {
        upgradeLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(UpgradingWall());
        }
    }
    public IEnumerator UpgradingWall()
    {
        Transform currentWalls = UIActor.Instance.baseMap.transform.Find($"Level {upgradeLevel} Wall");
        Transform nextWalls = UIActor.Instance.baseMap.transform.Find($"Level {upgradeLevel + 1} Wall");

        upgrading = true;

        nextWalls.gameObject.SetActive(true);
        UIActor.Instance.playerCam.SetActive(false);
        UIActor.Instance.baseCam.SetActive(true);
        RenderSettings.fog = false;

        for (int i = 0; i < currentWalls.childCount; i++)
        {
            currentWalls.GetChild(i).DOLocalMoveY(currentWalls.GetChild(i).position.y - 5, 1f).OnComplete(() =>
            {
                currentWalls.GetChild(i).gameObject.SetActive(false);
            });
            nextWalls.GetChild(i).transform.localPosition = new Vector3(nextWalls.GetChild(i).transform.localPosition.x, nextWalls.GetChild(i).transform.localPosition.y - 5f, nextWalls.GetChild(i).transform.localPosition.z);
            nextWalls.GetChild(i).DOLocalMoveY(nextWalls.GetChild(i).position.y + 5, 1f);
            nextWalls.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        UIActor.Instance.playerCam.SetActive(true);
        UIActor.Instance.baseCam.SetActive(false);
        RenderSettings.fog = true;

        upgradeLevel++;
        upgrading = false;
    }
}
