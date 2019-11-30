using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    float dAngle = 4f;
    float moveForce = 50f;
    float forceFac = 10f;
    float maxMoveSpeed = 8f;
    float liftSpeed = 2f;
    float maxElevatorHeight = 1.7f;
    float minElevatorHeight = 0.4f;

    float currAngle;
    float angleFac = 0;
    float forwardFac = 0;
    public bool panelPickup = false;
    public bool panelPlace = false;
    public bool cargo = false;
    bool lift = false;
    Transform elevator;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        elevator = transform.GetChild(1);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        angleFac = 0;
        forwardFac = 0;

        if (Input.GetKey("d")) {
            angleFac++;
        }

        if (Input.GetKey("a")) {
            angleFac--;
        }

        if (Input.GetKey("w")) {
            forwardFac++;
        }

        if (Input.GetKey("s")) {
            forwardFac--;
        }

        if (Input.GetKey("space")) {
            lift = true;
        } else {
            lift = false;
        }

        if (Input.GetKeyDown("e")) {
            panelPickup = true;
            panelPlace = true;
        }

        if (Input.GetKeyDown("q")) {
            cargo = true;
        }
    }

    void FixedUpdate() {
        //turn
        Vector3 angles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(angles.x, angles.y + angleFac * dAngle, angles.z);

        //move
        Vector3 targetVelocity = maxMoveSpeed * (transform.forward * forwardFac);
        Vector3 rbHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 targetForce = (targetVelocity - rbHorizontalVelocity) * forceFac;

        if (targetForce.magnitude > moveForce) {
            targetForce = targetForce.normalized * moveForce;
        }

        rb.AddForce(targetForce);

        //elevator
        float liftDist = liftSpeed * Time.deltaTime;
        Vector3 pos = elevator.localPosition;

        if (lift) {
            if (pos.y < maxElevatorHeight - liftDist) {
                pos.y += liftDist;
            } else {
                pos.y = maxElevatorHeight;
            }
        } else {
            if (pos.y > minElevatorHeight + liftDist) {
                pos.y -= liftDist;
            } else {
                pos.y = minElevatorHeight;
            }
        }

        elevator.localPosition = pos;
    }
}
