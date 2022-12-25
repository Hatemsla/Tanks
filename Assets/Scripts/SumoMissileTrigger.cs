using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoMissileTrigger : MonoBehaviour
{
    private float _pushPower = 500000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bot")
        {
            other.gameObject.GetComponentInParent<SumoTankAI>().Push(other.transform.position, _pushPower);
        }

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<SumoTankController>().Push(other.transform.position, _pushPower);
        }

        if (other.gameObject.tag != "Checkpoint")
            Destroy(gameObject);
    }
}
