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
    TextMeshProUGUI gameOverText;
    AudioManager audioManager;
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
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
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

    void moveUI (int On)
    {
        if (On <= 0) StartCoroutine(MoveUICoroutine(gridRect, OffPos, speed * Time.deltaTime));
        else StartCoroutine(MoveUICoroutine(gridRect, OnPos, speed * Time.deltaTime));
    }

    void perfUpdate(int state)
    {
        if(state == 0)
        {
            moveUI(1);
        }
        else if(state == 1)
        {
            robotsAlive.text += "YOU HAVE:\n" + gameManager.mostRecentOutput + " ROBOTS.";
            audioManager.playText();
        }
        else if (state == 2)
        {
            attackWarning.text = "YOU ARE\nATTACKED!";
            audioManager.playText(0, 3);
        }
        else if (state == 3)
        {
            int deadRobs = gameManager.mostRecentAttack - gameManager.mostRecentDamageDealt;
            killRobots(deadRobs);
            robotsDead.text += deadRobs + " DAMAGE IS BLOCKED BY ROBOTS.";
            if (deadRobs > 0) audioManager.playRobotDeath();
            else audioManager.playText();
        }
        else if (state == 4)
        {
            damageText.text += gameManager.mostRecentDamageDealt + " DAMAGE IS DEALT TO YOUR FACTORY.";
            //audioManager.playText();
            if(gameManager.mostRecentDamageDealt > 0) audioManager.playDamage();
        }
        else
        {
            robotsAlive.text = "";
            attackWarning.text = "";
            robotsDead.text = "";
            damageText.text = "";

            if (gameManager.health <= 0)
            {
                audioManager.playGameOver();
                gameOverText.text = "GAME OVER";
            }
            moveUI(0);
        }
    }
}
