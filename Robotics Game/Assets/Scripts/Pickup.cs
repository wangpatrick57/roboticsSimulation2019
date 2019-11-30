using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    bool cargoThisFrame = false;
    public bool pickedUpThisFrame;
    public GameObject heldCargo = null;
    public GameObject heldPanel = null;
    GameObject cargo;
    Transform elevatorTr;
    Transform robotTr;
    List<GameObject> inPickupCL = new List<GameObject>();
    Place placeSc;

    void Start() {
        elevatorTr = transform.parent.GetChild(1);
        robotTr = transform.parent;
        cargo = Resources.Load("Cargo") as GameObject;
        placeSc = elevatorTr.GetChild(1).GetComponent<Place>();
    }

    void OnTriggerEnter(Collider cl) {
        if (cl.gameObject != null) {
            inPickupCL.Add(cl.gameObject);
        }
    }

    void OnTriggerExit(Collider cl) {
        if (inPickupCL.Contains(cl.gameObject)) {
            inPickupCL.Remove(cl.gameObject);
        }
    }

    void Update() {
        if (robotTr.GetComponent<RobotBehavior>().cargo) {
            if (heldCargo == null) {
                if (heldPanel == null) {
                    foreach(GameObject o in inPickupCL) {
                        if (o.gameObject.tag == "Cargo") {
                            o.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                            heldCargo = o.gameObject;
                            break;
                        } else if (o.gameObject.tag == "LoadingStation") {
                            float cargoHeightOff = placeSc.cargoHeightOff;
                            float cargoForwardOff = placeSc.cargoForwardOff;
                            Vector3 cPos = elevatorTr.position + elevatorTr.up * cargoHeightOff + elevatorTr.forward * cargoForwardOff;
                            GameObject c = Instantiate(Resources.Load("Cargo"), cPos, o.transform.rotation) as GameObject;
                            c.GetComponent<Rigidbody>().isKinematic = true;
                            heldCargo = c.gameObject;
                            break;
                        }
                    }
                }
            } else {
                float cargoShootSpd = placeSc.cargoShootSpd;
                heldCargo.GetComponent<Rigidbody>().velocity = robotTr.GetComponent<Rigidbody>().velocity + transform.forward * cargoShootSpd;
                heldCargo.GetComponent<Rigidbody>().isKinematic = false;
                heldCargo = null;
            }

            robotTr.GetComponent<RobotBehavior>().cargo = false;
        }

        pickedUpThisFrame = false;

        if (robotTr.GetComponent<RobotBehavior>().panelPickup) {
            foreach(GameObject o in inPickupCL) {
                if (heldPanel == null) {
                    if (heldCargo == null) {
                        if (o.gameObject.tag == "Panel") {
                            if (!o.gameObject.GetComponent<PanelBehavior>().placed) {
                                o.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                o.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                                pickedUpThisFrame = true;
                                heldPanel = o.gameObject;
                                break;
                            }
                        } else if (o.gameObject.tag == "LoadingStation") {
                            float panelHeightOff = placeSc.panelHeightOff;
                            float panelForwardOff = placeSc.panelForwardOff;
                            Vector3 pPos = elevatorTr.position + elevatorTr.up * panelHeightOff + elevatorTr.forward * panelForwardOff;
                            GameObject p = Instantiate(Resources.Load("Panel"), pPos, o.transform.rotation) as GameObject;
                            p.GetComponent<Rigidbody>().isKinematic = true;
                            p.transform.rotation = new Quaternion(0, 0, 0, 0);
                            pickedUpThisFrame = true;
                            heldPanel = p.gameObject;
                            break;
                        }
                    }
                }

                robotTr.GetComponent<RobotBehavior>().panelPickup = false;
            }
        }
    }
}
