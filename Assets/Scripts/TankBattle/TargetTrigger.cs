using System.Collections;
using UnityEngine;

namespace TankBattle
{
    public class TargetTrigger : MonoBehaviour
    {
        public BattleController battleController;
        public MeshRenderer mesh;
        public BoxCollider objectCollider;
        public int index;
        public bool isSomeoneInZone;
        private int _score = 1;

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            objectCollider = GetComponent<BoxCollider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Bot" || other.gameObject.tag == "BotMissile")
            {
                battleController.CheckScore(_score, false);
                index = battleController.path.nodes.IndexOf(transform);
                battleController.path.nodes.Remove(transform);
                StartCoroutine(HideTarget());
            }
            else if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerMissile")
            {
                battleController.CheckScore(_score, true);
                index = battleController.path.nodes.IndexOf(transform);
                battleController.path.nodes.Remove(transform);
                StartCoroutine(HideTarget());
            }
        }

        private IEnumerator HideTarget()
        {
            mesh.enabled = false;
            objectCollider.enabled = false;
            yield return new WaitForSeconds(10);
            if (isSomeoneInZone)
            {
                StartCoroutine(HideTarget());
            }
            else
            {
                transform.localPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
                mesh.enabled = true;
                objectCollider.enabled = true;
                battleController.path.nodes.Insert(index, transform);
            }
        }
    }
}
