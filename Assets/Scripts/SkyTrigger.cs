using System.Collections;
using System.Collections.Generic;
using Sumo;
using UnityEngine;

public class SkyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bot")
        {
            var bot = other.gameObject.GetComponentInParent<SumoTankAI>();
            bot.isGround = false;
            bot.rb.drag = 0;
        }

        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponentInParent<SumoTankController>();
            player.isGround = false;
            player.rb.drag = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bot")
        {
            var bot = other.gameObject.GetComponentInParent<SumoTankAI>();
            bot.isGround = true;
            bot.rb.drag = 3f;
        }

        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponentInParent<SumoTankController>();
            player.isGround = true;
            player.rb.drag = 3f;
        }
    }
}
