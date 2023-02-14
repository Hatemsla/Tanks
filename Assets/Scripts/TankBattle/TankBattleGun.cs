using System.Collections;
using UnityEngine;

namespace TankBattle
{
    public class TankBattleGun : MonoBehaviour
    {
        public int maxMisslePower;
        public BattleController battleController;
        public GameObject missileObject;
        public Transform shootPosition;

        private bool _isShoot;

        private void Update()
        {
            if (battleController.isGameStart && Input.GetKeyDown(KeyCode.Q))
                Shoot();
        }

        private void Shoot()
        {
            if (!_isShoot)
            {
                _isShoot = true;
                var missile = Instantiate(missileObject, shootPosition.position, Quaternion.identity);
                missile.GetComponent<Rigidbody>().AddForce(shootPosition.forward * maxMisslePower, ForceMode.Impulse);
                missile.tag = "PlayerMissile";
                transform.GetComponentInParent<Rigidbody>().AddForce(shootPosition.forward * -maxMisslePower * 100, ForceMode.Impulse);
                StartCoroutine(IsShoot());
            }
        }

        private IEnumerator IsShoot()
        {
            yield return new WaitForSeconds(2);
            _isShoot = false;
        }
    }
}
