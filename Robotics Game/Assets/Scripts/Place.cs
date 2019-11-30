using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public float cargoHeightOff = -0f;
    public float cargoForwardOff = 0.2f;
    public float cargoShootSpd = 2.5f;
    public float panelHeightOff = 0f;
    public float panelForwardOff = 0.02f;
    float panelShootSpd = 5f;

    bool cargoThisFrame = false;
    bool placedThisFrame = false;
    GameObject heldCargo = null;
    GameObject heldPanel = null;
    GameObject cargo;
    Transform elevatorTr;
    Transform robotTr;
    Pickup pickupSc;
    List<GameObject> inPlaceCL = new List<GameObject>();

    void Start() {
        elevatorTr = transform.parent;
        robotTr = transform.parent.parent;
        cargo = Resources.Load("Cargo") as GameObject;
        pickupSc = robotTr.GetChild(0).GetComponent<Pickup>();
    }

    void OnTriggerEnter(Collider cl) {
        if (cl.gameObject != null) {
            inPlaceCL.Add(cl.gameObject);
        }
    }

    void OnTriggerExit(Collider cl) {
        if (inPlaceCL.Contains(cl.gameObject)) {
            inPlaceCL.Remove(cl.gameObject);
        }
    }

    void Update() {
        placedThisFrame = false;

        if (robotTr.GetComponent<RobotBehavior>().panelPlace) {
            foreach(GameObject o in inPlaceCL) {
                if (pickupSc.heldPanel != null && !pickupSc.pickedUpThisFrame) {
                    if (o.gameObject.tag == "PanelPlaceCL") {
                        if (o.GetComponent<PanelPlaceCLBehavior>().hasPanel == false) {
                            pickupSc.heldPanel.transform.position = o.gameObject.transform.position;
                            pickupSc.heldPanel.transform.rotation = o.gameObject.transform.rotation;
                            pickupSc.heldPanel.GetComponent<PanelBehavior>().placed = true;
                            o.GetComponent<PanelPlaceCLBehavior>().hasPanel = true;
                            pickupSc.heldPanel = null;
                            placedThisFrame = true;
                        }
                    }
                }
            }

            if (pickupSc.heldPanel != null && !placedThisFrame) {
                pickupSc.heldPanel.GetComponent<Rigidbody>().velocity = robotTr.GetComponent<Rigidbody>().velocity + transform.forward * panelShootSpd;
                pickupSc.heldPanel.GetComponent<Rigidbody>().isKinematic = false;
                pickupSc.heldPanel = null;
            }

            if (heldPanel != null && !pickupSc.pickedUpThisFrame) {
                heldPanel.GetComponent<Rigidbody>().velocity = robotTr.GetComponent<Rigidbody>().velocity + transform.forward * panelShootSpd;
                heldPanel.GetComponent<Rigidbody>().isKinematic = false;
                heldPanel = null;
            }

            robotTr.GetComponent<RobotBehavior>().panelPlace = false;
        }
    }

    void LateUpdate() {
        if (pickupSc.heldCargo != null) {
            pickupSc.heldCargo.transform.position = elevatorTr.position + elevatorTr.up * cargoHeightOff + elevatorTr.forward * cargoForwardOff;
        }

        if (pickupSc.heldPanel != null) {
            var elevatorEA = elevatorTr.rotation.eulerAngles;
            pickupSc.heldPanel.transform.position = elevatorTr.position + elevatorTr.up * panelHeightOff + elevatorTr.forward * panelForwardOff;
            pickupSc.heldPanel.transform.rotation = Quaternion.Euler(90 + elevatorEA.x, elevatorEA.y, elevatorEA.z);
        }
    }
}
