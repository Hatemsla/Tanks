using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMissileTrigger : MonoBehaviour
{
    private int _damage = 20;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bot")
        {
            other.gameObject.GetComponentInParent<TankBattleAI>().TakeDamage(_damage);
        }
        
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<TankBattleController>().TakeDamage(_damage);
        }

        if (other.gameObject.tag != "Checkpoint")
            Destroy(gameObject);

        // if (other.gameObject.tag == "Target")
        // {
        //     other.GetComponentInParent<PathAI>().nodes.Remove(other.transform);
        //     Destroy(other.gameObject);
        // }
    }
}
