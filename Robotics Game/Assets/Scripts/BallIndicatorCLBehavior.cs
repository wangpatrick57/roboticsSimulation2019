using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIndicatorCLBehavior : MonoBehaviour
{
    public bool first = false;
    float noneIntensity = 2;
    Color noneColor = new Color(1, 1, 1);
    float cargoIntensity = 20;
    Color cargoColor = new Color(255f / 255f, 162f / 255f, 19f / 255f);
    Light light;

    void Start() {
        if (first) {
            Vector3 tp = transform.position;
            Vector3 ta = transform.localEulerAngles;
            GameObject a = Instantiate(Resources.Load("BallIndicatorCL"), new Vector3(-tp.x, tp.y, tp.z), transform.rotation) as GameObject;
            GameObject b = Instantiate(Resources.Load("BallIndicatorCL"), new Vector3(tp.x, tp.y, -tp.z), transform.rotation) as GameObject;
            GameObject c = Instantiate(Resources.Load("BallIndicatorCL"), new Vector3(-tp.x, tp.y, -tp.z), transform.rotation) as GameObject;

            if (ta.y == 0) {
                a.transform.localEulerAngles = new Vector3(ta.x, ta.y + 180, ta.z);
                c.transform.localEulerAngles = new Vector3(ta.x, ta.y + 180, ta.z);
            } else {
                b.transform.localEulerAngles = new Vector3(ta.x, ta.y + 180, ta.z);
                c.transform.localEulerAngles = new Vector3(ta.x, ta.y + 180, ta.z);
            }
        }

        light = GetComponent<Light>();
        light.intensity = noneIntensity;
        light.color = noneColor;
    }

    void OnTriggerEnter(Collider cl) {
        if (cl.gameObject.tag == "Cargo") {
            light.intensity = cargoIntensity;
            light.color = cargoColor;
        }
    }

    void OnTriggerExit(Collider cl) {
        if (cl.gameObject.tag == "Cargo") {
            light.intensity = noneIntensity;
            light.color = noneColor;
        }
    }

    void Update() {
    }
}
