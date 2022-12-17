using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoTankAIGun : MonoBehaviour
{
    public int maxMisslePower;
    public Transform shootPosition;
    public GameObject missileObject;
    public GameObject targetController;
    public SumoController sumoController;
    public List<Transform> path;

    private bool _isShoot;

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, 100, -1, QueryTriggerInteraction.Ignore))
        {
            if (sumoController.isGameStart && path.Contains(hit.transform) && !_isShoot)
            {
                _isShoot = true;
                Shoot();
                StartCoroutine(IsShoot());
            }
        }   
    }

    private void Shoot()
    {
        var missile = Instantiate(missileObject, shootPosition.position, Quaternion.identity);
        missile.GetComponent<Rigidbody>().AddForce(shootPosition.forward * maxMisslePower, ForceMode.Impulse);
        transform.GetComponentInParent<Rigidbody>().AddForce(shootPosition.forward * -maxMisslePower * 100, ForceMode.Impulse);
    }

    private IEnumerator IsShoot()
    {
        yield return new WaitForSeconds(2);
        _isShoot = false;
    }
}
