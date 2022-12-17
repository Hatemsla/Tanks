using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTankGunAI : MonoBehaviour
{
    public int range;
    public Transform shootPosition;
    public LineRenderer laser;

    private bool _isShoot;

    private void Update()
    {
        if (!_isShoot)
        {
            Shoot();
            
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, range, -1, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.tag == "Wall")
            {
                _isShoot = true;
                LaserEffect(hit);
                Destroy(hit.transform.gameObject);
            }
            else if (hit.transform.tag == "Player")
            {
                _isShoot = true;
                LaserEffect(hit);
                StartCoroutine(SlowPlayer(hit));
            }
            else if (hit.transform.tag == "Bot")
            {
                _isShoot = true;
                LaserEffect(hit);
                StartCoroutine(SlowBot(hit));
            }
        }
    }

    private IEnumerator SlowPlayer(RaycastHit hit)
    {
        hit.transform.GetComponent<RaceTankController>().rb.drag = 3;
        yield return new WaitForSeconds(1);
        hit.transform.GetComponent<RaceTankController>().rb.drag = 0.05f;
    }

    private IEnumerator SlowBot(RaycastHit hit)
    {
        hit.transform.GetComponent<RaceTankAI>().rb.drag = 3;
        yield return new WaitForSeconds(1);
        hit.transform.GetComponent<RaceTankAI>().rb.drag = 0.05f;
    }

    private void LaserEffect(RaycastHit hit)
    {
        StartCoroutine(ShotEffect());
        StartCoroutine(IsShoot());
        laser.startColor = Color.red;
        laser.endColor = Color.red;
        laser.SetPosition(0, shootPosition.position);
        laser.SetPosition(1, shootPosition.position + shootPosition.forward * hit.distance);
    }

    private IEnumerator ShotEffect()
    {
        laser.enabled = true;
        yield return new WaitForSeconds(0.05f);
        laser.enabled = false;
    }

    private IEnumerator IsShoot()
    {
        yield return new WaitForSeconds(2);
        _isShoot = false;
    }
}
