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
        if (Physics.Raycast(transform.position, transform.forward, out hit, sensorLength))
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
}
