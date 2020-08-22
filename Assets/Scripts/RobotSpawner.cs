using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotSpawner : MonoBehaviour
{
    public GameObject robotObject;
    private Slider timeBar;
    private GameObject currRobot = null;
    public Vector3 spawnPos = new Vector3(-25, 0, 0);
    float timer;
    float TimerMax = 10f;

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
        if (currRobot == null && timer > 0) currRobot = Instantiate(robotObject, spawnPos, Quaternion.identity, this.transform);
        if (timer <= 0) EndZone();
    }

    void EndZone()
    {
        if(currRobot != null) Destroy(currRobot);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
