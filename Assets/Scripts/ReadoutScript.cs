using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ReadoutScript : MonoBehaviour
{

    public event Action doneEvent;
    private GameManager gameManager;
    private TextMeshProUGUI Values;
    private TextMeshProUGUI Part;
    private TextMeshProUGUI Dialogue;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindObjectOfType<GameManager>().robotCreated += SubscribeToRobot;
        FindObjectOfType<GameManager>().robotDestroyed += UnsubFromRobot;

        Values = GetChildWithName(gameObject, "Values").GetComponent<TextMeshProUGUI>();
        Part = GetChildWithName(gameObject, "Part").GetComponent<TextMeshProUGUI>();
        Dialogue = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
    }

    public void BroadcastDone()
    {
        doneEvent();
    }

    public void SubscribeToRobot()
    {
        gameManager.currRobotScript.updateReadout += UpdateStrings;
        gameManager.currRobotScript.updateDialogue += UpdateDialogue;
    }

    public void UnsubFromRobot()
    {
        gameManager.currRobotScript.GetComponent<RobotRepair>().updateReadout -= UpdateStrings;
        gameManager.currRobotScript.GetComponent<RobotRepair>().updateDialogue -= UpdateDialogue;
    }

    public void UpdateStrings(string values, string part)
    {
        Part.text = part;
        Values.text = values;
    }

    public void UpdateDialogue(string dialogue)
    {
        Dialogue.text = dialogue;
    }

    //gets a child of a given gameobject with a given name, if one exists
    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
