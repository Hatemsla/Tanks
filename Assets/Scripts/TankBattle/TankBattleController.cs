using System.Collections;
using UnityEngine;

namespace TankBattle
{
    public class TankBattleController : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        public bool isGround = true;
        public bool isSleep;
        public BattleController battleController;
        public Transform towerTransform;
        public Rigidbody rb;
        public WheelCollider[] rightWheelsCol;
        public WheelCollider[] leftWheelsCol;
        public Transform[] rightWheelTrans;
        public Transform[] leftWheelTrans;

        private float _wheelSteer = 100;
        private float _healthRestorationTime = 2;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            currentHealth = maxHealth;
        }

        private void FixedUpdate()
        {
            if (isGround && battleController.isGameStart && !isSleep)
            {
                TankMove();
            }
        }

        private void TankMove()
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.X))
                {
                    transform.Translate(new Vector3(1, 0, 1) * Time.deltaTime * 5);
                    rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                    leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    transform.Translate(new Vector3(-1, 0, 1) * Time.deltaTime * 5);
                    leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                }
                else
                {
                    transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * 5);
                    leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                    leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.X))
                {
                    transform.Translate(new Vector3(1, 0, -1) * Time.deltaTime * 5);
                    leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    transform.Translate(new Vector3(-1, 0, -1) * Time.deltaTime * 5);
                    rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                    leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                }
                else
                {
                    transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * 5);
                    leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                    leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                    rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                }
            }
            else if (Input.GetKey(KeyCode.X))
            {
                transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 5);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 5);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(0, 1, 0), -Time.deltaTime * 50);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 50);
                leftWheelTrans[0].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                leftWheelTrans[1].Rotate(new Vector3(1, 0, 0), -Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[0].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
                rightWheelTrans[1].Rotate(new Vector3(1, 0, 0), Time.deltaTime * _wheelSteer, Space.Self);
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            battleController.SetHealthBar(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                battleController.CheckScore(2, false);
                StartCoroutine(TankSleep());
            }
        }

        private IEnumerator RestoreHealth()
        {
            float timeRemaining = _healthRestorationTime;

            while (timeRemaining > 0)
            {
                float multiplier = Mathf.Clamp01(1 - timeRemaining / _healthRestorationTime);
                currentHealth = maxHealth * multiplier;

                battleController.SetHealthBar(currentHealth);
                timeRemaining -= Time.deltaTime;

                yield return null;
            }
            currentHealth = maxHealth;
        }

        private IEnumerator TankSleep()
        {
            isSleep = true;
            yield return RestoreHealth();
            isSleep = false;
        }
    }
}
