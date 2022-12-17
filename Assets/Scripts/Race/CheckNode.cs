using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            foreach (GameObject wall in roundTrigger.walls)
            {
                Destroy(wall);
            }
            roundTrigger.walls = new List<GameObject>();
        }

        int i = 1;
        foreach (Transform wall in roundTrigger.wallSpawns)
        {
            var wallObj = Instantiate(roundTrigger.wallPrefab, wall.localPosition,
                Quaternion.identity);
            wallObj.transform.rotation = wall.localRotation;
            wallObj.transform.position = wall.position;
            roundTrigger.walls.Add(wallObj);
            i++;
        }
    }
}
