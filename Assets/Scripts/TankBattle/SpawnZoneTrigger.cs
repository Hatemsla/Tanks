using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneTrigger : MonoBehaviour
{
    public TargetTrigger targetTrigger;

    private int _objectsInZone = 0;

    private void Start()
    {
        targetTrigger = GetComponentInChildren<TargetTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
        {
            _objectsInZone++;
            CheckZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
        {
            _objectsInZone--;
            CheckZone();
        }
    }

    private void CheckZone()
    {
        if (_objectsInZone == 0)
        {
            targetTrigger.isSomeoneInZone = false;
        }
        else
        {
            targetTrigger.isSomeoneInZone = true;
        }
    }
}
