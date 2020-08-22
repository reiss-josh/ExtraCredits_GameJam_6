using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//takes a spriteSheet and randomly assigns a sprite therein to the current spriterenderer
public class SelectRandomSpriteFromSheet : MonoBehaviour
{
    public Texture2D spriteSheet;
    private Sprite[] singleSprites;
    private SpriteRenderer sRender;

    void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
        singleSprites = Resources.LoadAll<Sprite>("Sprites/" + spriteSheet.name);
        int spriteChosen = Random.Range(0, singleSprites.Length);
        sRender.sprite = singleSprites[spriteChosen];
    }
}
