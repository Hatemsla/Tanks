using System.Collections.Generic;
using UnityEngine;

public class RaceTankAI : MonoBehaviour
{
    public float maxMotorTorque;
    public float maxBrakeForce;
    public float maxSteerAngle;
    public float turnSpeed;
    public float maxSpeed;
    public float currentSpeed;
    public int currentNode;
    public bool isBraking;
    public Rigidbody rb;
    public CheckNode checkNode;
    public PathAI pathAI;
    public RaceController raceController;
    public WheelCollider[] frontWheelsCollider;
    public WheelCollider[] rearWheelsCollider;
    public Transform[] frontWheelsTransform;
    public Transform[] rearWheelsTransform;

    private float _targetSteerAngle;
    private bool _reversing;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        checkNode = GetComponent<CheckNode>();
        checkNode.nodes = pathAI.nodes;
    }

    private void Update()
    {
        currentNode = checkNode.currentNode;
    }

    private void FixedUpdate()
    {
        if (raceController.isGameStart)
        {
            // Sensors();
            ApplySteer();
            Drive();
            UpdateAllWheelPoses();
            Braking();
            LerpToSteerAngle();
        }
    }

    private void LerpToSteerAngle()
    {
        frontWheelsCollider[0].steerAngle = Mathf.Lerp(frontWheelsCollider[0].steerAngle, _targetSteerAngle, Time.deltaTime * turnSpeed);
        frontWheelsCollider[1].steerAngle = Mathf.Lerp(frontWheelsCollider[1].steerAngle, _targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * frontWheelsCollider[0].radius * frontWheelsCollider[0].rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            if (!_reversing)
            {
                foreach (WheelCollider wheel in frontWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in rearWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
            }
            else
            {
                foreach (WheelCollider wheel in frontWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in rearWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
            }
        }
        else
        {
            foreach (WheelCollider wheel in frontWheelsCollider)
            {
                wheel.motorTorque = 0;
            }
            foreach (WheelCollider wheel in rearWheelsCollider)
            {
                wheel.motorTorque = 0;
            }
        }
    }

    private void ApplySteer()
    {
        // if (_avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(checkNode.nodes[currentNode].position);
        float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;
        _targetSteerAngle = newSteer;
    }

    private void Braking()
    {
        if (isBraking)
        {
            foreach (WheelCollider wheel in frontWheelsCollider)
            {
                wheel.brakeTorque = maxBrakeForce;
            }
            foreach (WheelCollider wheel in rearWheelsCollider)
            {
                wheel.brakeTorque = maxBrakeForce;
            }
        }
        else
        {
            foreach (WheelCollider wheel in frontWheelsCollider)
            {
                wheel.brakeTorque = 0;
            }
            foreach (WheelCollider wheel in rearWheelsCollider)
            {
                wheel.brakeTorque = 0;
            }
        }
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