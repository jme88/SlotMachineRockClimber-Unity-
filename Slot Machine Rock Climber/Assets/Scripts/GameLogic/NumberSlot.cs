using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSlot : MonoBehaviour {

    private int numSlot;

    public void Start()
    {
        numSlot = Int32.Parse(this.gameObject.name);
    }

    public int numeroSlot()
    {
        return numSlot;
    }
}
