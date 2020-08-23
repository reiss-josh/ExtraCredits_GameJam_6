using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsCanvasController : MonoBehaviour
{
    Vector3 resIconHolderoffPos = new Vector3(1200f, 0f, 0f);
    Vector3 resIconHolderonPos = new Vector3(0f, 0f, 0f);
    public float resIconMoveSpeed = 1000f;
    RectTransform resultIconsHolderTf;
    TextMeshProUGUI successCounter;
    TextMeshProUGUI okayCounter;
    TextMeshProUGUI failureCounter;

    void Start()
    {
        //get components in children
        resultIconsHolderTf = GetChildWithName(gameObject, "ResultIconsHolder").GetComponent<RectTransform>();
        successCounter = GameObject.Find("SuccessCounter").GetComponent<TextMeshProUGUI>();
        failureCounter = GameObject.Find("FailureCounter").GetComponent<TextMeshProUGUI>();

        resultIconsHolderTf.anchoredPosition = resIconHolderoffPos;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.updateCanvasses += UpdatePositioning;
        gameManager.updateResultsText += UpdateResultsText;
    }

    void UpdateResultsText(int good, int bad)
    {
        successCounter.text = "X" + good.ToString();
        failureCounter.text = "X" + bad.ToString();
    }

    void UpdatePositioning(int canvasSet)
    {
        MoveResultsIcons(canvasSet == 1);
    }

    IEnumerator MoveUICoroutine(RectTransform element, Vector3 target, float step)
    {
        while(Vector2.Distance(element.anchoredPosition, target) > 0.05f)
        {
            element.anchoredPosition = Vector2.MoveTowards(element.anchoredPosition, target, step);
            yield return null;
        }
    }

    void MoveResultsIcons(bool moveOnScreen = false)
    {
        Vector3 destination = moveOnScreen ? resIconHolderonPos : resIconHolderoffPos;
        StartCoroutine(MoveUICoroutine(resultIconsHolderTf, destination, resIconMoveSpeed * Time.deltaTime));
    }

    void UpdateResultsText()
    {

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
