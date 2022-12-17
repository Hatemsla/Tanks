using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTankController : MonoBehaviour
{
    public float maxMotorTorque;
    public float maxBrakeForce;
    public float maxSteerAngle;
    public Rigidbody rb;
    public CheckNode checkNode;
    public PathAI pathAI;
    public RaceController raceController;
    public WheelCollider[] frontWheelsCollider;
    public WheelCollider[] rearWheelsCollider;
    public Transform[] frontWheelsTransform;
    public Transform[] rearWheelsTransform;

    private float _verticalInput;
    private float _horizontalInput;
    private float _brakeInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        checkNode = GetComponent<CheckNode>();
        checkNode.nodes = pathAI.nodes;
    }

    private void FixedUpdate()
    {
        if (raceController.isGameStart)
        {
            GetInput();
            Drive();
            UpdateAllWheelPoses();
        }
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _brakeInput = Input.GetAxis("Brake");
    }

    private void Drive()
    {
        foreach (WheelCollider wheel in frontWheelsCollider)
        {
            wheel.motorTorque = maxMotorTorque * _verticalInput * Time.deltaTime * 100;
            wheel.brakeTorque = maxBrakeForce * _brakeInput;
            wheel.steerAngle = maxSteerAngle * _horizontalInput;
        }
        foreach (WheelCollider wheel in rearWheelsCollider)
        {
            wheel.motorTorque = maxMotorTorque * _verticalInput * Time.deltaTime * 100;
            wheel.brakeTorque = maxBrakeForce * _brakeInput;
        }
        UpdateAllWheelPoses();

    }

    private void UpdateAllWheelPoses()
    {
        UpdateWheelPose(rearWheelsCollider[0], rearWheelsTransform[0]);
        UpdateWheelPose(rearWheelsCollider[1], rearWheelsTransform[1]);
        UpdateWheelPose(frontWheelsCollider[0], frontWheelsTransform[0]);
        UpdateWheelPose(frontWheelsCollider[1], frontWheelsTransform[1]);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
