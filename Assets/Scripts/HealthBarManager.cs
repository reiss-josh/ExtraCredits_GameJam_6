using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    Vector3 OffPos = new Vector3(1310, -440, 0), OnPos = new Vector3(685, -440, 0);
    float speed = 1000f;
    RectTransform gridRect;
    // Start is called before the first frame update
    void Start()
    {
        gridRect = GetComponent<RectTransform>();
        GameObject.Find("GameManager").GetComponent<GameManager>().battleSystemUpdate += moveUI;
    }

    IEnumerator MoveUICoroutine(RectTransform element, Vector3 target, float step)
    {
        while (Vector2.Distance(element.anchoredPosition, target) > 0.05f)
        {
            element.anchoredPosition = Vector2.MoveTowards(element.anchoredPosition, target, step);
            yield return null;
        }
    }

    void moveUI(int On)
    {
        if (On > 4 || On < 0) StartCoroutine(MoveUICoroutine(gridRect, OffPos, speed * Time.deltaTime));
        else if (On == 0) StartCoroutine(MoveUICoroutine(gridRect, OnPos, speed * Time.deltaTime));
    }
}
