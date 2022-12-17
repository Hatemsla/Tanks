using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBattleAI : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float maxMotorTorque = 100;
    public float maxSteerAngle = 45f;
    public int currentNode;
    public int passedNode;
    public float wayDistance;
    public bool isGround = true;
    public bool isDead;
    public GameObject path;
    public BattleController battleController;
    public Rigidbody rb;
    public List<Transform> nodes;
    public WheelCollider[] FrontWheelsCol;
    public WheelCollider[] RearWheelsCol;
    public Transform[] FrontWheelTrans;
    public Transform[] RearWheelTrans;

    private float _targetSteerAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nodes = path.GetComponent<PathAI>().nodes;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isGround && battleController.isGameStart && !isDead)
        {
            ApplySteer();
            Drive();
            CheckWaypointDistance();
            LerpToSteerAngle();
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
            if (currentNode == nodes.Count - 1)
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
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;
        _targetSteerAngle = newSteer;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }
}
