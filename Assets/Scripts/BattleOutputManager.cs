using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BattleOutputManager : MonoBehaviour
{
    TextMeshProUGUI robotsAlive;
    TextMeshProUGUI attackWarning;
    TextMeshProUGUI robotsDead;
    TextMeshProUGUI damageText;
    GameManager gameManager;
    private RectTransform gridRect;
    public float speed = 1000f;
    private Vector3 OnPos = new Vector3(-20, 0, 0), OffPos = new Vector3(600, 0, 0);

    public event Action<int> killRobots;
    // Start is called before the first frame update
    void Start()
    {
        robotsAlive = GameObject.Find("RobotsAliveOutput").GetComponent<TextMeshProUGUI>();
        attackWarning = GameObject.Find("AttackWarning").GetComponent<TextMeshProUGUI>();
        robotsDead = GameObject.Find("RobotDeathOutput").GetComponent<TextMeshProUGUI>();
        damageText = GameObject.Find("DamageOutput").GetComponent<TextMeshProUGUI>();
        gridRect = GetComponent<RectTransform>();

        robotsAlive.text = "";
        attackWarning.text = "";
        robotsDead.text = "";
        damageText.text = "";

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.battleSystemUpdate += perfUpdate;
    }

    IEnumerator MoveUICoroutine(RectTransform element, Vector3 target, float step)
    {
        while (Vector2.Distance(element.anchoredPosition, target) > 0.05f)
        {
            element.anchoredPosition = Vector2.MoveTowards(element.anchoredPosition, target, step);
            yield return null;
        }
    }

    void moveUI (int numRobots)
    {
        if (numRobots <= 0) StartCoroutine(MoveUICoroutine(gridRect, OffPos, speed * Time.deltaTime));
        else StartCoroutine(MoveUICoroutine(gridRect, OnPos, speed * Time.deltaTime));
    }

    void perfUpdate(int state)
    {
        if(state == 0)
        {
            moveUI(5);
            robotsAlive.text += "YOU HAVE:\n" + gameManager.mostRecentOutput + " ROBOTS.";
        }
        else if (state == 1)
        {
            attackWarning.text = "YOU ARE\nATTACKED!";
        }
        else if (state == 2)
        {
            int deadRobs = gameManager.mostRecentAttack - gameManager.mostRecentDamageDealt;
            killRobots(deadRobs);
            robotsDead.text += deadRobs + " DAMAGE IS BLOCKED BY ROBOTS.";
        }
        else if (state == 3)
        {

            damageText.text += gameManager.mostRecentDamageDealt + " DAMAGE IS DEALT TO YOUR FACTORY.";
        }
        else
        {
            robotsAlive.text = "";
            attackWarning.text = "";
            robotsDead.text = "";
            damageText.text = "";
            moveUI(0);
        }
    }
}
