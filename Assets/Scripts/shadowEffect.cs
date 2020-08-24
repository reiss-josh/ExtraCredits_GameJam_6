using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class shadowEffect : MonoBehaviour
{
    GameObject _shadow;
    public Vector3 Offset = new Vector3(-0.1f, -0.1f);
    public Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = transform;

        _shadow.transform.localPosition = Offset;
        _shadow.transform.localRotation = Quaternion.identity;
        _shadow.transform.localScale = Vector3.one;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = _shadow.AddComponent<SpriteRenderer>();
        sr.sprite = renderer.sprite;
        sr.material = renderer.material;
        sr.color = myColor;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        _shadow.transform.localPosition = Offset;
    }
}
