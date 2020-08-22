using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    public GameObject carriedChild;
    public Vector3 startPos = new Vector3(-20f, 8.5f, 0);
    public float moveSpeed = 5f;
    public float threshold = 0.1f;

    private Rigidbody2D rb2d;
    private Rigidbody2D childRb;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        rb2d = GetComponent<Rigidbody2D>();
        childRb = carriedChild.GetComponent<Rigidbody2D>();
        childRb.isKinematic = true;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isApproxEqual(transform.position.x, 0, threshold)) AwakenChild();
        rb2d.velocity = Vector3.right * moveSpeed;
        Debug.Log(childRb.IsSleeping());
    }

    public bool isApproxEqual(float value1, float value2, float threshold)
    {
        return Mathf.Abs(value1 - value2) <= threshold;
    }

    void AwakenChild()
    {
        carriedChild.transform.parent = null;
        childRb.isKinematic = false;
    }
}
