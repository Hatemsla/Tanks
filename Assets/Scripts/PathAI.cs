using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PathAI : MonoBehaviour
{
    public List<Transform> nodes;

    private void Awake()
    {
        nodes = GetComponentsInChildren<Transform>().ToList();
        nodes.RemoveAt(0);
    }
}
