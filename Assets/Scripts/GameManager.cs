using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //prefabs
    public GameObject robotObject;
    private GameObject currRobot = null;
    public RobotRepair currRobotScript;

    //acquired ui elements
    private Slider timeBar;
    

    //regular ol variables
    public Vector3 spawnPos = new Vector3(-25, 0, 0);
    float timer;
    public float TimerMax = 10f;
    private int[] mostRecentResults = new int[] { 0, 0};
    public int currZone = 0;

    //events
    public event System.Action robotCreated;
    public event System.Action robotDestroyed;
    public event System.Action<int> updateCanvasses;
    public event System.Action<int, int> updateResultsText;

    // Start is called before the first frame update
    void Start()
    {
        timer = TimerMax;
        timeBar = GameObject.Find("TimeBar").GetComponent<Slider>();
        if(updateCanvasses != null) updateCanvasses(0);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timeBar.value = timer / TimerMax;
        if (currRobot == null && timer > 0 && currZone == 0)
        {
            currRobot = Instantiate(robotObject, spawnPos, Quaternion.identity, this.transform);
            currRobotScript = currRobot.GetComponent<RobotRepair>();
            currRobotScript.exitEvent += DestroyChild;
            currRobotScript.updateStatus += UpdateResultsForRound;


            robotCreated();
        }
        if (timer <= 0 && currZone == 0) EndRepairZone();
        if (timer <= 0 && currZone == 1) EndCombatZone();
    }

    public void UpdateResultsForRound(int status)
    {
        int ind = 0;
        if (status < 0) ind = 1;
        mostRecentResults[ind] += 1;
        Debug.Log(mostRecentResults[ind]);
    }

    public void DestroyChild()
    {
        currRobotScript.exitEvent -= DestroyChild;
        currRobotScript.updateStatus -= UpdateResultsForRound;
        robotDestroyed();
        Destroy(currRobot);
    }

    void EndRepairZone()
    {
        currZone = 1;
        if(currRobot != null) DestroyChild();
        updateCanvasses(currZone);
        updateResultsText(mostRecentResults[0], mostRecentResults[1]);
        timer = 5f;
    }

    void EndResultsZone()
    {
        currZone = 2;
        updateCanvasses(currZone);
    }

    void EndCombatZone()
    {
        currZone = 0;
        updateCanvasses(currZone);
        timer = TimerMax;
        mostRecentResults = new int[] { 0, 0, 0 };
    }
}
