using System.Collections;
using System.Collections.Generic;
using TankBattle;
using UnityEngine;

public class BattleMissileTrigger : MonoBehaviour
{
    private int _damage = 20;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bot"))
        {
            var bot = other.gameObject.GetComponentInParent<TankBattleAI>();
            if (!bot.isSleep)
            {
                bot.TakeDamage(_damage);
                
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponentInParent<TankBattleController>();
            if (!player.isSleep)
            {
                player.TakeDamage(_damage);
                
            }
        }

        if (!other.gameObject.CompareTag("Checkpoint"))
            Destroy(gameObject);

    }
}
