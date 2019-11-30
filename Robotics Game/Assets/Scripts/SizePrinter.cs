using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePrinter : MonoBehaviour
{
    void Start() {
        Debug.Log(GetComponent<Collider>().bounds.size);
    }
}
