using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointID;

    private void Start()
    {
        checkpointID = int.Parse(gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.GetComponentInParent<RaceTankController>();
            if (player.checkNode.currentNode == checkpointID)
                player.checkNode.CheckWaypoint(); // расчет дистанции до следующего чекпоинта
        }
        if (other.gameObject.tag == "Bot")
        {
            var bot = other.GetComponentInParent<RaceTankAI>();
            if (bot.checkNode.currentNode == checkpointID)
                bot.checkNode.CheckWaypoint();
        }
    }
}
