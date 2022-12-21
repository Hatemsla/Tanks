using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float distance;
    public float sensorLength;
    public RaycastHit hit;

    public bool CheckObstacle()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, sensorLength, -1, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, hit.point);
            if (!hit.transform.CompareTag("Terrain"))
            {
                Debug.DrawLine(transform.position, hit.point);
                distance = hit.distance;
                return true;
            }
            return false;
        }
        return false;
    }

    public bool CheckTarget()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, sensorLength, -1, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.transform.CompareTag("Target"))
            {
                Debug.DrawLine(transform.position, hit.point);
                distance = hit.distance;
                return true;
            }
            return false;
        }
        return false;
    }
}
