using UnityEngine;

namespace Race
{
    public class RaceTankAI : MonoBehaviour
    {
        public float maxMotorTorque;
        public float maxBrakeForce;
        public float maxSteerAngle;
        public float turnSpeed;
        public float avoidSpeed;
        public float maxSpeed;
        public float currentSpeed;
        public bool isBraking;
        public bool isKnocked;
        public Rigidbody rb;
        public CheckNode checkNode;
        public PathAI pathAI;
        public RaceController raceController;
        public Sensor frontSensor;
        public Sensor rightFrontSensor;
        public Sensor leftFrontSensor;
        public Sensor rightAngleSensor;
        public Sensor leftAngleSensor;
        public Sensor rightSideSensor;
        public Sensor leftSideSensor;
        public WheelCollider[] frontWheelsCollider;
        public WheelCollider[] rearWheelsCollider;
        public Transform[] frontWheelsTransform;
        public Transform[] rearWheelsTransform;

        private float _targetSteerAngle;
        private float _respawnTime = 2;
        private float _respawnCounter = 0;
        private float _reversCounter;
        private float _waitToReverse = 2.0f;
        private float _reversFor = 1.5f;
        private bool _reversing;
        private bool _avoiding;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0, -1, 0);
            checkNode = GetComponent<CheckNode>();
            checkNode.nodes = pathAI.nodes;
        }

        private void FixedUpdate()
        {
            if (raceController.isGameStart)
            {
                Sensors();
                ApplySteer();
                Drive();
                UpdateAllWheelPoses();
                Braking();
                LerpToSteerAngle();
                Respawn();
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

        private void Sensors()
        {
            float avoidMultiplier = 0;
            _avoiding = false;

            if (rightFrontSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier -= 1f;
            }
            else if (rightAngleSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier -= 0.5f;
            }

            if (rightSideSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier -= 0.5f;
            }

            if (leftFrontSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier += 1f;
            }
            else if (leftAngleSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier += 0.5f;
            }

            if (leftSideSensor.CheckObstacle())
            {
                _avoiding = true;
                avoidMultiplier += 0.5f;
            }

            if (frontSensor.CheckObstacle())
            {
                _avoiding = true;
                if (frontSensor.hit.normal.x < 0)
                {
                    avoidMultiplier = -1;
                }
                else
                {
                    avoidMultiplier = 1;
                }
            }

            if (rb.velocity.magnitude < 2 && !_reversing && !isKnocked)
            {
                _reversCounter += Time.deltaTime;
                if (_reversCounter >= _waitToReverse)
                {
                    _reversCounter = 0;
                    _reversing = true;
                    isBraking = false;
                }
            }

            if (_reversing)
            {
                avoidMultiplier *= -1;
                _reversCounter += Time.deltaTime;
                if (_reversCounter >= _reversFor)
                {
                    _reversCounter = 0;
                    _reversing = false;
                }
            }

            if (_avoiding)
            {
                _targetSteerAngle = avoidSpeed * avoidMultiplier;
            }
        }

        private void ApplySteer()
        {
            if (_avoiding) return;
            Vector3 relativeVector = transform.InverseTransformPoint(checkNode.nodes[checkNode.currentNode].position);
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

        private void Respawn()
        {
            Vector3 targetNode;
            if (rb.velocity.magnitude < 2 && !isKnocked)
            {
                _respawnCounter += Time.deltaTime;
                if (_respawnCounter >= _respawnTime)
                {
                    if (checkNode.currentNode == 0)
                    {
                        transform.position = checkNode.nodes[checkNode.nodes.Count - 1].position;
                        targetNode = new Vector3(checkNode.nodes[0].position.x, transform.position.y,
                            checkNode.nodes[0].position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(targetNode, Vector3.up);
                    }
                    else
                    {
                        transform.position = checkNode.nodes[checkNode.currentNode - 1].position;
                        targetNode = new Vector3(checkNode.nodes[checkNode.currentNode].position.x, transform.position.y,
                            checkNode.nodes[checkNode.currentNode].position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(targetNode, Vector3.up);
                    }
                    _respawnCounter = 0;
                    _reversCounter = 0;
                    _reversing = false;
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
}