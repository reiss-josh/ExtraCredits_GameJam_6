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
    private AudioManager audioManager;

    //acquired ui elements
    private Slider timeBar;
    private Slider healthBar;
    

    //regular ol variables
    public Vector3 spawnPos = new Vector3(-25, 0, 0);
    float timer;
    public float[] timerMaxArr = { 30f, 3f, 9f, 1.25f }; //repair, results, combat, delay
    public float[] battleTimerMarks = { 9f, 8f, 6f, 4f, 2f, 0f};
    public int mostRecentOutput = 0;
    public int mostRecentAttack = 0;
    public int mostRecentDamageDealt = 0;
    private int[] mostRecentResults = new int[] { 0, 0};
    public int currZone = 0;
    public float health;
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
        timer = timerMaxArr[0];
        timeBar = GameObject.Find("TimeBar").GetComponent<Slider>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(updateCanvasses != null) updateCanvasses(0);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Cancel")) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
        if(currZone == 0) timeBar.value = timer / timerMaxArr[0];

        if (timer <= 0 && currZone == 0) EndRepairZone(); // 0 is repair screen - for end
        else if (timer <= 0 && currZone == 1) EndResultsZone(); //1 is results screen - for end
        else if (timer <= 0 && currZone == 2) EndCombatZone(); //2 is combat screen - for end
        else if (currZone == 2) AdvanceCombatZone(); //2 is combat screen - for during
        else if (timer <= 0 && currZone == -1) DelayScreen(); //-1 is delay before results
        else if (timer <= 0 && currZone == -2) DelayScreen(); //-2 is delay before combat
        else if (timer <= 0 && currZone == -3) DelayScreen(); //-3 is delay before repair screen / game over
        else if (timer <= 0 && currZone == -4) GameOver();
    }

    int combatState = -1;
    void AdvanceCombatZone()
    {
        bool isUpdating = false;
        if(timer < battleTimerMarks[combatState+1] && combatState == 3) { health -= mostRecentDamageDealt; }
        if (timer < battleTimerMarks[combatState+1]) { isUpdating = true; combatState++; }
        if (isUpdating) battleSystemUpdate(combatState);
    }

    public void UpdateResultsForRound(int status)
    {
        if (status < 0) mostRecentResults[1] += 1;
        else mostRecentResults[0] += 1;
        Debug.Log("status, from manager: " + status);
    }

    int DetermineNewestAttack()
    {
        return currWave + (int)(currWave/5f) + 1;
        //return Mathf.Clamp(2*(currWave), 1, 16);
    }

    public void DestroyChild()
    {
        //if (lastResults == mostRecentResults) UpdateResultsForRound(currRobotScript.status); //sanity check
        currRobotScript.exitEvent -= DestroyChild;
        currRobotScript.updateStatus -= UpdateResultsForRound;
        robotDestroyed();
        Destroy(currRobot);
    }

    void EndRepairZone()
    {
        currZone = -1;
        updateCanvasses(currZone);

        if (currRobot != null) DestroyChild(); //kill robots onscreen
        
        updateResultsText(mostRecentResults[0], mostRecentResults[1]);
        timer = timerMaxArr[3];
    }

    void EndResultsZone()
    {
        currZone = -2;
        updateCanvasses(currZone);
        mostRecentOutput = Mathf.Clamp(mostRecentResults[0] - mostRecentResults[1], 0, 99);
        mostRecentAttack = DetermineNewestAttack();
        mostRecentDamageDealt = Mathf.Clamp(mostRecentAttack - mostRecentOutput, 0, 99);
        if (robotCountUpdate != null) robotCountUpdate(mostRecentOutput);
        timer = timerMaxArr[3];
    }

    void EndCombatZone()
    {
        currZone = -3; //move to next screen
        updateCanvasses(currZone); //perform relevant updates

        battleSystemUpdate(5); //update battle canvas
        combatState = -1; //reset combat state

        //if (robotCountUpdate != null) robotCountUpdate(0); //remove when done 
        mostRecentResults = new int[] { Mathf.Clamp(mostRecentOutput-mostRecentAttack, 0, 99), 0};
        currWave++;
        GameObject.Find("CurrDayText").GetComponent<TMPro.TextMeshProUGUI>().text = "CURRENT DAY: " + currWave;
        timer = timerMaxArr[3];
    }

    void DelayScreen()
    {
        currZone = ((currZone * -1) % 3);
        timer = timerMaxArr[currZone];
        CheckForGameOver();

        updateCanvasses(currZone);
    }

    void CheckForGameOver()
    {
        if(health <= 0)
        {
            currZone = -4;
            timer = timerMaxArr[3];
        }
    }

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
