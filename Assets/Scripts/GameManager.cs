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
    private Slider healthBar;
    

    //regular ol variables
    public Vector3 spawnPos = new Vector3(-25, 0, 0);
    float timer;
    public float TimerMax = 10f;
    public float resultsTime = 3f;
    public float[] battleTimerMarks = { 10f, 7.5f, 5f, 2.5f};
    public int mostRecentOutput = 0;
    public int mostRecentAttack = 0;
    public int mostRecentDamageDealt = 0;
    private int[] mostRecentResults = new int[] { 0, 0};
    public int currZone = 0;
    float health;
    public int maxHealth = 10;
    public int currWave = 0;

    //events
    public event System.Action robotCreated;
    public event System.Action robotDestroyed;
    public event System.Action<int> updateCanvasses;
    public event System.Action<int, int> updateResultsText;
    public event System.Action<int> robotCountUpdate;
    public event System.Action<int> battleSystemUpdate;

    // Start is called before the first frame update
    void Start()
    {
        timer = TimerMax;
        timeBar = GameObject.Find("TimeBar").GetComponent<Slider>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        if(updateCanvasses != null) updateCanvasses(0);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health/maxHealth;
        MakeNewRobot();
        UpdateTimer();
    }

    private void MakeNewRobot()
    {
        if (currRobot == null && timer > 0 && currZone == 0)
        {
            currRobot = Instantiate(robotObject, spawnPos, Quaternion.identity, this.transform);

            currRobotScript = currRobot.GetComponent<RobotRepair>();
            currRobotScript.exitEvent += DestroyChild;
            currRobotScript.updateStatus += UpdateResultsForRound;

            robotCreated();
        }
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timeBar.value = timer / TimerMax;
        if (timer <= 0 && currZone == 0) EndRepairZone();
        else if (timer <= 0 && currZone == 1) EndResultsZone();
        else if (currZone == 2) AdvanceCombatZone();
    }

    int combatState = -1;
    void AdvanceCombatZone()
    {
        bool isUpdating = false;
        if(timer < battleTimerMarks[0] && combatState < 0) { isUpdating = true; combatState++; }
        else if (timer < battleTimerMarks[1] && combatState == 0) { isUpdating = true; combatState++; }
        else if (timer < battleTimerMarks[2] && combatState == 1) { isUpdating = true; combatState++; }
        else if (timer < battleTimerMarks[3] && combatState == 2) { isUpdating = true; combatState++; health -= mostRecentDamageDealt; }
        if (isUpdating) battleSystemUpdate(combatState);

        if (timer < 0) EndCombatZone();
    }

    public void UpdateResultsForRound(int status)
    {
        int ind = 0;
        if (status < 0) ind = 1;
        mostRecentResults[ind] += 1;
        Debug.Log("status updated");
    }

    int DetermineNewestAttack()
    {
        return currWave;
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
        timer = resultsTime;
    }

    void EndResultsZone()
    {
        currZone = 2;
        updateCanvasses(currZone);
        mostRecentOutput = Mathf.Clamp(mostRecentResults[0] - mostRecentResults[1], 0, 99);
        mostRecentAttack = DetermineNewestAttack();
        mostRecentDamageDealt = Mathf.Clamp(mostRecentAttack - mostRecentOutput, 0, 999);
        if (robotCountUpdate != null) robotCountUpdate(mostRecentOutput);
        timer = battleTimerMarks[0];
    }

    void EndCombatZone()
    {
        battleSystemUpdate(4);
        combatState = -1;
        currZone = 0;
        updateCanvasses(currZone);
        timer = TimerMax;
        if (robotCountUpdate != null) robotCountUpdate(0); //remove when done
        mostRecentResults = new int[] { 0, 0, 0 };
        currWave++;
        GameObject.Find("CurrDayText").GetComponent<TMPro.TextMeshProUGUI>().text = "CURRENT DAY: " + currWave;
        CheckForGameOver();
    }

    void CheckForGameOver()
    {
        if(health <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
