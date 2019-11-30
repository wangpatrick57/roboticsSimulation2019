﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoWallBehavior : MonoBehaviour
{
    public bool first = false;

    // Start is called before the first frame update
    void Start()
    {
        if (first) {
            Vector3 tp = transform.position;
            Instantiate(Resources.Load("CargoWall"), new Vector3(tp.x, tp.y, -tp.z), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
