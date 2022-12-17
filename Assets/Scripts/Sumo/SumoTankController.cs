using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoTankController : MonoBehaviour
{
    public bool isGround = true;
    public Transform towerTransform;
    public Rigidbody rb;
    public SumoController sumoController;
    public WheelCollider[] rightWheelsCol;
    public WheelCollider[] leftWheelsCol;
    public Transform[] rightWheelTrans;
    public Transform[] leftWheelTrans;

    private float _wheelSteer = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isGround && sumoController.isGameStart && !sumoController.isRoundOver)
        {
            TankMove();
        }
    }

    private void TankMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // if (Input.GetKey(KeyCode.D))
            // {
            //     transform.Translate(new Vector3(1, 0, 1) * Time.deltaTime * 5);
            //     rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            //     leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            // }
            // else if (Input.GetKey(KeyCode.A))
            // {
            //     transform.Translate(new Vector3(-1, 0, 1) * Time.deltaTime * 5);
            //     leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            //     rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            // }
            // else
            // {
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * 5);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            // }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // if (Input.GetKey(KeyCode.D))
            // {
            //     transform.Translate(new Vector3(1, 0, -1) * Time.deltaTime * 5);
            //     leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            //     rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            // }
            // else if (Input.GetKey(KeyCode.A))
            // {
            //     transform.Translate(new Vector3(-1, 0, -1) * Time.deltaTime * 5);
            //     rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            //     leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            // }
            // else
            // {
                transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * 5);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            // }
        }
        // else if (Input.GetKey(KeyCode.D))
        // {
        //     transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 5);
        //     leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
        //     leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
        //     rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
        //     rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
        // }
        // else if (Input.GetKey(KeyCode.A))
        // {
        //     transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 5);
        //     leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
        //     leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
        //     rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
        //     rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
        // }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 1, 0), -Time.deltaTime * 50);
            leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 50);
            leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
        }
    }

    public void Push(Vector3 pushFrom, float pushPower)
    {
        var pushDirection = (pushFrom - transform.position).normalized;
        rb.AddForce(pushDirection * pushPower);
    }
}
