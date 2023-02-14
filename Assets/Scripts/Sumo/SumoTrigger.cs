using UnityEngine;

namespace Sumo
{
    public class SumoTrigger : MonoBehaviour
    {
        public SumoController sumoController;

        private void Start()
        {
            sumoController.GetComponent<SumoController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Bot")
            {
                var bot = other.gameObject.GetComponentInParent<SumoTankAI>();
                bot.isGround = false;
                bot.rb.drag = 0;
                sumoController.isWin = true;
                sumoController.CheckMatchResult();
                sumoController.isRoundOver = true;
            }

            if (other.gameObject.tag == "Player")
            {
                var player = other.gameObject.GetComponentInParent<SumoTankController>();
                player.isGround = false;
                player.rb.drag = 0;
                sumoController.CheckMatchResult();
                sumoController.isRoundOver = true;
            }
        }
    }
}
