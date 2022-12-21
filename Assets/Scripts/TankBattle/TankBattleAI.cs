using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBattleAI : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float maxMotorTorque;
    public float maxSteerAngle;
    public float avoidSpeed;
    public int currentNode;
    public int passedNode;
    public float wayDistance;
    public bool isGround = true;
    public bool isSleep;
    public GameObject path;
    public BattleController battleController;
    public Rigidbody rb;
    public Sensor frontSensor;
    public Sensor rightFrontSensor;
    public Sensor leftFrontSensor;
    public Sensor rightAngleSensor;
    public Sensor leftAngleSensor;
    public Sensor rightSideSensor;
    public Sensor leftSideSensor;
    public List<Transform> nodes;
    public WheelCollider[] FrontWheelsCol;
    public WheelCollider[] RearWheelsCol;
    public Transform[] FrontWheelTrans;
    public Transform[] RearWheelTrans;

    private float _targetSteerAngle;
    private bool _isAvoiding;
    private bool _isTargetFind;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nodes = path.GetComponent<PathAI>().nodes;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isGround && battleController.isGameStart && !isSleep)
        {
            TargetSensors();
            if(!_isTargetFind)
                AvoidingSensors();
            ApplySteer();
            Drive();
            CheckWaypointDistance();
            LerpToSteerAngle();
        }
    }

    private void TargetSensors()
    {
        float avoidMultiplier = 0;
        _isAvoiding = false;
        _isTargetFind = false;

        if (rightFrontSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier += 1f;
        }
        else if (rightAngleSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier += 0.5f;
        }

        if (rightSideSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier += 0.5f;
        }

        if (leftFrontSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier -= 1f;
        }
        else if (leftAngleSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier -= 0.5f;
        }

        if (leftSideSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            avoidMultiplier += 0.5f;
        }

        if (frontSensor.CheckTarget())
        {
            _isAvoiding = true;
            _isTargetFind = true;
            if (frontSensor.hit.normal.x < 0)
            {
                avoidMultiplier = 1;
            }
            else
            {
                avoidMultiplier = -1;
            }
        }

        if (_isTargetFind)
        {
            _targetSteerAngle = avoidSpeed * avoidMultiplier;
        }
    }

    private void AvoidingSensors()
    {
        float avoidMultiplier = 0;
        _isAvoiding = false;

        if (rightFrontSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier -= 1f;
        }
        else if (rightAngleSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier -= 0.5f;
        }

        if (rightSideSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier -= 0.5f;
        }

        if (leftFrontSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier += 1f;
        }
        else if (leftAngleSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier += 0.5f;
        }

        if (leftSideSensor.CheckObstacle())
        {
            _isAvoiding = true;
            avoidMultiplier += 0.5f;
        }

        if (frontSensor.CheckObstacle())
        {
            _isAvoiding = true;
            if (frontSensor.hit.normal.x < 0)
            {
                avoidMultiplier = -1;
            }
            else
            {
                avoidMultiplier = 1;
            }
        }

        if (_isAvoiding)
        {
            _targetSteerAngle = avoidSpeed * avoidMultiplier;
        }
    }

    private void LerpToSteerAngle()
    {
        if (_targetSteerAngle < 3 && _targetSteerAngle > -3)
        {
            transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * _targetSteerAngle);
            FrontWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * maxMotorTorque, Space.Self);
            RearWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * maxMotorTorque, Space.Self);
            FrontWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * maxMotorTorque, Space.Self);
            RearWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * maxMotorTorque, Space.Self);
        }
        else if (_targetSteerAngle > 0)
        {
            transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * _targetSteerAngle);
            FrontWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            RearWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            FrontWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            RearWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _targetSteerAngle * 10, Space.Self);
        }
        else if (_targetSteerAngle < 0)
        {
            transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * _targetSteerAngle);
            FrontWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            RearWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            FrontWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _targetSteerAngle * 10, Space.Self);
            RearWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _targetSteerAngle * 10, Space.Self);
        }

    }

    public void CheckWaypointDistance()
    {
        wayDistance = Vector3.Distance(transform.position, nodes[currentNode].position);
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 20f)
        {
            if (currentNode == nodes.Count - 1 || currentNode >= nodes.Count - 1)
                currentNode = 0;
            else
                currentNode++;
        }
    }

    private void Drive()
    {
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * 5);
    }

    private void ApplySteer()
    {
        if (_isAvoiding || _isTargetFind) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;
        _targetSteerAngle = newSteer;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            battleController.CheckScore(2, true);
            StartCoroutine(TankSleep());
        }
    }

    private IEnumerator TankSleep()
    {
        isSleep = true;
        yield return new WaitForSeconds(2);
        currentHealth = maxHealth;
        isSleep = false;
    }
}
