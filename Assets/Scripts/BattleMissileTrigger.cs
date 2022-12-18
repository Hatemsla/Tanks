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
            var bot = other.gameObject.GetComponentInParent<TankBattleAI>();
            if (!bot.isSleep)
            {
                bot.TakeDamage(_damage);
                
            }
        }

        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponentInParent<TankBattleController>();
            if (!player.isSleep)
            {
                player.TakeDamage(_damage);
                
            }
        }

        if (other.gameObject.tag != "Checkpoint")
            Destroy(gameObject);

    }
}
