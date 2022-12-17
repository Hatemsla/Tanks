using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTankGun : MonoBehaviour
{
    public int range;
    public Transform shootPosition;
    public LineRenderer laser;

    private bool _isShoot;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isShoot)
        {
            _isShoot = true;
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
                Destroy(hit.transform.gameObject);
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
        hit.transform.GetComponent<RaceTankAI>().rb.drag = 3;
        yield return new WaitForSeconds(1);
        hit.transform.GetComponent<RaceTankAI>().rb.drag = 0.05f;
    }

    private IEnumerator ShootEffect()
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
