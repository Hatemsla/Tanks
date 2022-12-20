using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathAI : MonoBehaviour
{
    public List<Transform> nodes;

    private void Awake()
    {
        GetNodes();
    }

    public void GetNodes()
    {
        nodes = GetComponentsInChildren<Transform>().ToList();
        nodes.RemoveAt(0);
    }
}
