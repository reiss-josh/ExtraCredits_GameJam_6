using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGridPopulator : MonoBehaviour
{
    public GameObject RobotIcon;
    public int numRobots;
    private List<GameObject> robots = new List<GameObject>();

    public float speed = 1000f;
    private RectTransform gridRect;
    private Vector3 robotGridOffPos = new Vector3(-615, 0, 0), robotGridOnPos = new Vector3(-114f, 0, 0);

    void Start()
    {
        GameObject.Find("BattleOutputPanel").GetComponent<BattleOutputManager>().killRobots += KillRobots;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.robotCountUpdate += UpdateRobotCount;
        
        gridRect = GetComponent<RectTransform>();
    }

    void UpdateRobotCount(int newCount)
    {
        for (int i = 0; i < robots.Count; i++)
        {
            Destroy(robots[i]);
        }
        robots.Clear();
        numRobots = newCount;
        if (numRobots <= 0) StartCoroutine(MoveUICoroutine(gridRect, robotGridOffPos, speed * Time.deltaTime));
        else StartCoroutine(MoveUICoroutine(gridRect, robotGridOnPos, speed * Time.deltaTime));
        PopulateGrid();
    }

    IEnumerator MoveUICoroutine(RectTransform element, Vector3 target, float step)
    {
        while (Vector2.Distance(element.anchoredPosition, target) > 0.05f)
        {
            element.anchoredPosition = Vector2.MoveTowards(element.anchoredPosition, target, step);
            yield return null;
        }
    }

    void PopulateGrid()
    {
        for(int i = 0; i < numRobots; i++)
        {
            GameObject newRobot = Instantiate(RobotIcon, transform);
            robots.Add(newRobot);
        }
    }

    void KillRobots(int numDeadRobots)
    {
        for(int i = 0; i < Mathf.Min(numDeadRobots, robots.Count); i++)
        {
            RectTransform currRobTransform = robots[i].GetComponent<RectTransform>();
            currRobTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
    }
}
