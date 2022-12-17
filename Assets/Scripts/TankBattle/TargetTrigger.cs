using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public BattleController battleController;
    private int _score = 1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bot")
        {
            battleController.CheckScore(_score, false);
            battleController.targetTriggers.Remove(this);
            battleController.path.nodes.Remove(this.transform);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            battleController.CheckScore(_score, true);
            battleController.targetTriggers.Remove(this);
            battleController.path.nodes.Remove(this.transform);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "PlayerMissile")
        {
            battleController.CheckScore(_score, true);
            battleController.targetTriggers.Remove(this);
            battleController.path.nodes.Remove(this.transform);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "BotMissile")
        {
            battleController.CheckScore(_score, false);
            battleController.targetTriggers.Remove(this);
            battleController.path.nodes.Remove(this.transform);
            Destroy(gameObject);
        }
    }
}
