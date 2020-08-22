using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadoutScript : MonoBehaviour
{

    public event Action doneEvent;

    public void BroadcastDone()
    {
        doneEvent();
    }
}
