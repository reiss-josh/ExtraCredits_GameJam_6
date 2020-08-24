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
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currSprite = GetComponent<SpriteRenderer>();
        defaultSprite = currSprite.sprite;
    }

    void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
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
            audioManager.playTool();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeTool(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeTool(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeTool(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeTool(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeTool(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeTool(5);
    }

    public void ChangeTool(int newTool)
    {
        audioManager.playTool();
        currTool = newTool;
        currSprite.sprite = toolSprites[newTool];
    }
}