using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
    SpriteRenderer currSprite;
    public int currTool = -1;
    Sprite defaultSprite;
    public Sprite[] toolSprites;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currSprite = GetComponent<SpriteRenderer>();
        defaultSprite = currSprite.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isFocused)
        {
            Cursor.visible = false;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;
        }
        if (Input.GetButton("Fire2"))
        {
            currTool = -1;
            currSprite.sprite = defaultSprite;
        }
    }

    public void ChangeTool(int newTool)
    {
        currTool = newTool;
        currSprite.sprite = toolSprites[newTool];
    }
}
