using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoTankGun : MonoBehaviour
{
    public int maxMisslePower;
    public GameObject missileObject;
    public Transform shootPosition;
    public SumoController sumoController;

    private bool _isShoot;

    private void Update()
    {
        if (sumoController.isGameStart && Input.GetMouseButtonDown(0))
            Shoot();
    }

    private void Shoot()
    {
        if (!_isShoot)
        {
            _isShoot = true;
            var missile = Instantiate(missileObject, shootPosition.position, Quaternion.identity);
            missile.GetComponent<Rigidbody>().AddForce(shootPosition.forward * maxMisslePower, ForceMode.Impulse);
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
