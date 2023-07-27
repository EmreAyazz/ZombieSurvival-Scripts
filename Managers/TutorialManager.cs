using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<GameObject> queue;
    public LineRenderer lineRenderer;
    Player player;

    public GameObject ilkFence;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (queue.Count > 0)
        {
            if (queue[0] == null)
                queue.RemoveAt(0);

            float distance = Vector3.Distance(player.transform.position, queue[0].transform.position);

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(player.transform.position, queue[0].transform.position, 1, path);
            Vector3[] corners = path.corners;

            for (int i = 0; i < corners.Length; i++)
            {
                corners[i].y = 0.5f;
            }

            lineRenderer.gameObject.SetActive(true);
            lineRenderer.SetPositions(corners);
            lineRenderer.SetWidth(1.5f, 1.5f);

            if (distance <= 5)
            {
                queue.RemoveAt(0);
            }
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
            lineRenderer.SetPositions(new Vector3[0]);
        }
    }

    public void FenceKapat()
    {
        if (ilkFence)
        {
            ilkFence.GetComponent<Fence>().openingObject.SetActive(true);
            ilkFence.GetComponent<Fence>().companion.SetActive(false);
            ilkFence.GetComponent<Fence>().companion.transform.parent.GetChild(1).gameObject.SetActive(false);
            ilkFence.GetComponent<Fence>().companion.transform.parent.parent.GetChild(4).gameObject.SetActive(false);
            ilkFence.GetComponent<Fence>().opened = true;
        }
    }
}
