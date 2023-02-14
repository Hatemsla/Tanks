using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Race
{
    public class Wall : MonoBehaviour
    {
        public List<Transform> walls;

        private void Start()
        {
            walls = gameObject.GetComponentsInChildren<Transform>().ToList();
            walls.RemoveAt(0);
        }
    }
}
