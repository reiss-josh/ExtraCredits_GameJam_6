using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGridPopulator : MonoBehaviour
{
    public GameObject RobotIcon;
    public int numRobots;
    private List<GameObject> robots = new List<GameObject>();

    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().robotCountUpdate += UpdateRobotCount;
    }

    void UpdateRobotCount(int newCount)
    {
        for (int i = 0; i < robots.Count; i++)
        {
            Destroy(robots[i]);
        }
        robots.Clear();
        numRobots = newCount;
        PopulateGrid();
    }

    void PopulateGrid()
    {
        for(int i = 0; i < numRobots; i++)
        {
            GameObject newRobot = Instantiate(RobotIcon, transform);
            robots.Add(newRobot);
        }
    }
}
