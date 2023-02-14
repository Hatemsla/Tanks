using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class RoundTrigger : MonoBehaviour
    {
        public bool isFirstLap = true;
        public GameObject wallPrefab;
        public Transform wallSpawns;
        public RaceController raceController;
        public List<Wall> walls;
        public List<WallSpawn> wallSpawnsList;

        private void Start()
        {
            foreach (Transform wallSpawn in wallSpawns)
            {
                var wall = wallSpawn.gameObject.GetComponent<WallSpawn>();
                wall.roundTrigger = this;
                wall.wallPrefab = wallPrefab;
                wallSpawnsList.Add(wall);
            }
        }

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
}
