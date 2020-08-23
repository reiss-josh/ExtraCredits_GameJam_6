using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCanvasController : MonoBehaviour
{
    Vector3 timeInactivePos = new Vector3(275, -125, 0),
            dialogueInactivePos = new Vector3(0,290,0),
            readoutInactivePos = new Vector3(205,0,0),
            toolsInactivePos = new Vector3(-200,0,0),
            currDayInactivePos = new Vector3(-200,-145,0);
    Vector3 timeActivePos = new Vector3(-275, -125, 0),
            dialogueActivePos = Vector3.zero,
            readoutActivePos = new Vector3(-350, 0, 0),
            toolsActivePos = new Vector3(250, 0, 0),
            currDayActivePos = new Vector3(250, -145, 0);
    RectTransform timeBar, dialogue, readoutScreen, toolsBar, currDay;

    public float speed = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.updateCanvasses += UpdateUIPositioning;

        timeBar = GetChildWithName(gameObject, "TimeBar").GetComponent<RectTransform>();
        dialogue = GetChildWithName(gameObject, "RobotDialogueHolder").GetComponent<RectTransform>();
        readoutScreen = GetChildWithName(gameObject, "ReadoutHolder").GetComponent<RectTransform>();
        toolsBar = GetChildWithName(gameObject, "ToolsHolder").GetComponent<RectTransform>();
        currDay = GetChildWithName(gameObject, "CurrDayHolder").GetComponent<RectTransform>();

        UpdateUIPositioning(0);
    }

    IEnumerator MoveUICoroutine(RectTransform element, Vector3 target, float step)
    {
        while (Vector2.Distance(element.anchoredPosition, target) > 0.05f)
        {
            element.anchoredPosition = Vector2.MoveTowards(element.anchoredPosition, target, step);
            yield return null;
        }
    }

    void UpdateUIPositioning(int canvasSet)
    {
        bool thisUIisActive = (canvasSet == 0);
        Vector3 timeDest = thisUIisActive ? timeActivePos : timeInactivePos;
        Vector3 dialogueDest = thisUIisActive ? dialogueActivePos : dialogueInactivePos;
        Vector3 readoutDest = thisUIisActive ? readoutActivePos : readoutInactivePos;
        Vector3 toolsDest = thisUIisActive ? toolsActivePos : toolsInactivePos;
        Vector3 currDayDest = thisUIisActive ? currDayActivePos : currDayInactivePos;

        StartCoroutine(MoveUICoroutine(timeBar, timeDest, speed * Time.deltaTime));
        StartCoroutine(MoveUICoroutine(dialogue, dialogueDest, speed * Time.deltaTime));
        StartCoroutine(MoveUICoroutine(readoutScreen, readoutDest, speed * Time.deltaTime));
        StartCoroutine(MoveUICoroutine(toolsBar, toolsDest, speed * Time.deltaTime));
        StartCoroutine(MoveUICoroutine(currDay, currDayDest, speed * Time.deltaTime));
    }

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
