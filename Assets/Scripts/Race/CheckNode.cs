using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class CheckNode : MonoBehaviour
    {
        public int currentNode = 0;
        public int passedNode = 0;
        public int currentLap = 1;
        public float wayDistance;
        public RaceController raceController;
        public RoundTrigger roundTrigger;
        public List<Transform> nodes;

        private bool _isFirstRound = true;

        private void Update()
        {
            wayDistance = Vector3.Distance(transform.position, nodes[currentNode].position);
        }

        public void CheckWaypoint()
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
            passedNode++;
        }

        public void LapCount()
        {
            if (!_isFirstRound)
            {
                currentLap++;
            }
            _isFirstRound = false;
        }

        public void SpawnWall()
        {
            if (roundTrigger.walls.Count > 0)
            {
                for (int i = 0; i < roundTrigger.wallSpawnsList.Count; i++)
                {
                    roundTrigger.wallSpawnsList[i].SetRandomPosition();
                }
            }

            if (roundTrigger.isFirstLap)
            {
                for (int i = 0; i < roundTrigger.wallSpawnsList.Count; i++)
                {
                    roundTrigger.wallSpawnsList[i].SpawnWall();
                }
                roundTrigger.isFirstLap = false;
            }
        }
    }
}
