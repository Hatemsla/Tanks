using UnityEngine;

namespace Race
{
    public class RaceTankController : MonoBehaviour
    {
        public float maxMotorTorque;
        public float maxBrakeForce;
        public float maxSteerAngle;
        public float maxSpeed;
        public float currentSpeed;
        public bool isKnocked;
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
        private float _respawnTime = 2;
        private float _respawnCounter = 0;

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
                GetInput();
                Drive();
                Respawn();
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
            currentSpeed = 2 * Mathf.PI * frontWheelsCollider[0].radius * frontWheelsCollider[0].rpm * 60 / 1000;

            if (currentSpeed <= maxSpeed)
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

        private void Respawn()
        {
            Vector3 targetNode;
            if (Input.GetKey(KeyCode.R) && !isKnocked && rb.velocity.magnitude < 2)
            {
                _respawnCounter += Time.deltaTime;
                if (_respawnCounter >= _respawnTime)
                {
                    if (checkNode.currentNode == 0)
                    {
                        transform.position = new Vector3(checkNode.nodes[checkNode.nodes.Count - 1].position.x,
                            0.5f, checkNode.nodes[checkNode.nodes.Count - 1].position.z);
                        targetNode = new Vector3(checkNode.nodes[0].position.x, checkNode.nodes[0].position.y,
                            checkNode.nodes[0].position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(targetNode, Vector3.up);
                    }
                    else
                    {
                        transform.position = new Vector3(checkNode.nodes[checkNode.currentNode - 1].position.x, 0.5f,
                            checkNode.nodes[checkNode.currentNode - 1].position.z);
                        targetNode = new Vector3(checkNode.nodes[checkNode.currentNode].position.x,
                            checkNode.nodes[checkNode.currentNode].position.y,
                            checkNode.nodes[checkNode.currentNode].position.z) - transform.position;
                        transform.rotation = Quaternion.LookRotation(targetNode, Vector3.up);
                    }
                    _respawnCounter = 0;
                }
            }
            else
            {
                _respawnCounter = 0;
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
