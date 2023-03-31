using UnityEngine;

namespace Sumo
{
    public class SumoMissileTrigger : MonoBehaviour
    {
        private float _pushPower = 500000f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bot"))
            {
                other.gameObject.GetComponentInParent<SumoTankAI>().Push(other.transform.position, _pushPower);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<SumoTankController>().Push(other.transform.position, _pushPower);
            }

            if (!other.gameObject.CompareTag("Checkpoint"))
                Destroy(gameObject);
        }
    }
}
