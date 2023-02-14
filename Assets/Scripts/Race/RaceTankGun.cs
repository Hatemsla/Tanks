using System.Collections;
using UnityEngine;

namespace Race
{
    public class RaceTankGun : MonoBehaviour
    {
        public int range;
        public Transform shootPosition;
        public LineRenderer laser;
        public RaceController raceController;

        private bool _isShoot;

        private void Start()
        {
            raceController = FindObjectOfType<RaceController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !_isShoot && raceController.isGameStart)
            {
                Shoot();
                StartCoroutine(IsShoot());
            }
        }

        private void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, range, -1, QueryTriggerInteraction.Ignore))
            {
                StartCoroutine(ShootEffect());
                laser.startColor = Color.red;
                laser.endColor = Color.red;
                laser.SetPosition(0, shootPosition.position);
                laser.SetPosition(1, shootPosition.position + shootPosition.forward * hit.distance);
                if (hit.transform.tag == "Wall")
                {
                    hit.transform.gameObject.SetActive(false);
                }
                else if (hit.transform.tag == "Bot")
                {
                    StartCoroutine(SlowBot(hit));
                }
            }
            else
            {
                StartCoroutine(ShootEffect());
                laser.startColor = Color.red;
                laser.endColor = Color.red;
                laser.SetPosition(0, shootPosition.position);
                laser.SetPosition(1, shootPosition.position + shootPosition.forward * range);
            }
        }

        private IEnumerator SlowBot(RaycastHit hit)
        {
            var bot = hit.transform.GetComponent<RaceTankAI>();
            bot.rb.drag = 3;
            bot.isKnocked = true;
            yield return new WaitForSeconds(1);
            bot.rb.drag = 0.05f;
            yield return new WaitForSeconds(2);
            bot.isKnocked = false;
        }

        private IEnumerator ShootEffect()
        {
            laser.enabled = true;
            yield return new WaitForSeconds(0.05f);
            laser.enabled = false;
        }

        private IEnumerator IsShoot()
        {
            _isShoot = true;
            yield return new WaitForSeconds(2);
            _isShoot = false;
        }
    }
}
