using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTankGunAI : MonoBehaviour
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
        if (!_isShoot && raceController.isGameStart)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, range, -1, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Wall") && hit.distance < 20)
            {
                LaserEffect(hit);
                Destroy(hit.transform.gameObject);
            }
            else if (hit.transform.CompareTag("Player"))
            {
                LaserEffect(hit);
                StartCoroutine(SlowPlayer(hit));
            }
            else if (hit.transform.CompareTag("Bot"))
            {
                LaserEffect(hit);
                StartCoroutine(SlowBot(hit));
            }
        }
    }

    private IEnumerator SlowPlayer(RaycastHit hit)
    {
        var player = hit.transform.GetComponent<RaceTankController>();
        player.rb.drag = 3;
        player.isKnocked = true;
        yield return new WaitForSeconds(1);
        player.rb.drag = 0.05f;
        player.isKnocked = false;
    }

    private IEnumerator SlowBot(RaycastHit hit)
    {
        var bot = hit.transform.GetComponent<RaceTankAI>();
        bot.rb.drag = 3;
        bot.isKnocked = true;
        yield return new WaitForSeconds(1);
        bot.rb.drag = 0.05f;
        bot.isKnocked = false;
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
        _isShoot = true;
        yield return new WaitForSeconds(5);
        _isShoot = false;
    }
}
