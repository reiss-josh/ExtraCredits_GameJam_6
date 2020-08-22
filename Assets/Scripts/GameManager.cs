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
    float timer, TimerMax = 60f;

    //events
    public event System.Action robotCreated;
    public event System.Action robotDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        timer = TimerMax;
        timeBar = GameObject.Find("TimeBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timeBar.value = timer / TimerMax;
        if (currRobot == null && timer > 0)
        {
            currRobot = Instantiate(robotObject, spawnPos, Quaternion.identity, this.transform);
            currRobotScript = currRobot.GetComponent<RobotRepair>();
            currRobotScript.exitEvent += DestroyChild;
            currRobotScript.updateStatus += UpdateResultsForRound;


            robotCreated();
        }
        if (timer <= 0) EndZone();
    }

    public void UpdateResultsForRound(int status)
    {
        Debug.Log(status);
    }

    public void DestroyChild()
    {
        currRobotScript.exitEvent -= DestroyChild;
        currRobotScript.updateStatus -= UpdateResultsForRound;
        robotDestroyed();
        Destroy(currRobot);
    }

    void EndZone()
    {
        if(currRobot != null) Destroy(currRobot);
        Debug.Log("time up!!");
    }
}
