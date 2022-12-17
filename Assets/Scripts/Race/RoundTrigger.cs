using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTrigger : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform wallSpawns;
    public RaceController raceController;
    public List<GameObject> walls;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var car = other.gameObject.GetComponentInParent<RaceTankController>();
            if (car.checkNode.currentNode == 1)
                car.checkNode.LapCount();
        }
        if (other.gameObject.tag == "Bot")
        {
            var bot = other.gameObject.GetComponentInParent<RaceTankAI>();
            if (bot.checkNode.currentNode == 1)
                bot.checkNode.LapCount();
        }

        var firstTank = raceController.tanks.IndexOf(other.GetComponentInParent<CheckNode>());
        if (firstTank == 0)
        {
            raceController.tanks[firstTank].SpawnWall();
        }
    }
}
