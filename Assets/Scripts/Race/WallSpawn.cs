using UnityEngine;

namespace Race
{
    public class WallSpawn : MonoBehaviour
    {
        public GameObject wallPrefab;
        public Transform wallBlock;
        public RoundTrigger roundTrigger;
        public int tankCounter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
            {
                tankCounter++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
            {
                tankCounter--;
            }
        }

        public void SpawnWall()
        {
            var wallObj = Instantiate(roundTrigger.wallPrefab, transform.position, Quaternion.identity);
            wallObj.transform.SetParent(transform);
            wallObj.transform.rotation = transform.rotation;
            wallObj.transform.position = transform.position;
            wallObj.transform.localPosition = new Vector3(0, 0, Random.Range(-0.5f, 0.5f));
            wallObj.transform.localScale = new Vector3(0.15f, 1.4f, 0.01f);
            wallBlock = wallObj.transform;
            roundTrigger.walls.Add(wallObj.GetComponent<Wall>());
        }

        public void SetRandomPosition()
        {
            if (tankCounter == 0)
            {
                foreach (Transform wall in wallBlock)
                {
                    wall.gameObject.SetActive(true);
                }
                wallBlock.transform.localPosition = new Vector3(0, 0, Random.Range(-0.5f, 0.5f));
            }
        }
    }
}
