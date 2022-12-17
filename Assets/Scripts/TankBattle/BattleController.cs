using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    public bool isGameStart;
    public TankBattleController player;
    public TankBattleAI bot;
    public Timer timer;
    public PathAI path;
    public BattleUIManager battleUIManager;
    public List<TargetTrigger> targetTriggers;

    private int _botScore = 0;
    private int _playerScore = 0;

    private void Start()
    {
        player.GetComponent<TankBattleController>();
        player.battleController = this;
        player.GetComponentInChildren<TankBattleGun>().battleController = this;
        battleUIManager.healthBar.maxValue = player.maxHealth;
        battleUIManager.healthBar.value = player.maxHealth;
        bot.GetComponent<TankBattleAI>();
        bot.battleController = GetComponent<BattleController>();
        bot.GetComponentInChildren<TankBattleAIGun>().battleController = this;
        bot.GetComponentInChildren<TankBattleAIGun>().path = path.nodes;
        bot.nodes = path.nodes;
        battleUIManager.timeToStartGameText.gameObject.SetActive(true);
        battleUIManager.scoreText.text = $"{_playerScore} | {_botScore}";

        foreach (var item in path.nodes)
        {
            TargetTrigger target;
            item.TryGetComponent<TargetTrigger>(out target);
            if (target is TargetTrigger)
            {
                targetTriggers.Add(target);
                target.battleController = this;
            }
        }
    }

    private void Update()
    {
        CheckStartGame();
        CheckEndGame();
    }

    private void CheckStartGame()
    {
        if (timer.waitForStartGame >= 0)
        {
            battleUIManager.timeToStartGameText.text = $"До начала осталось: {timer.waitForStartGame:f1}";
        }
        else
        {
            battleUIManager.timeToStartGameText.gameObject.SetActive(false);
            battleUIManager.descriptionPanel.SetActive(false);
            isGameStart = true;
        }
    }

    private void CheckEndGame()
    {
        if (timer.waitForEndGame >= 0 && targetTriggers.Count > 0 && !bot.isDead)
        {
            float minutes = Mathf.FloorToInt(timer.waitForEndGame / 60);
            float seconds = Mathf.FloorToInt(timer.waitForEndGame % 60);
            battleUIManager.timeToEndGameText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            if (bot.isDead && !player.isDead)
                battleUIManager.matchResultText.text = "Вы победили!";
            else if (player.isDead && !bot.isDead)
                battleUIManager.matchResultText.text = "Жаль, но вы проиграли";
            else if (player.isDead && bot.isDead)
                battleUIManager.matchResultText.text = "Ничья!";
            else
            {
                if (_botScore > _playerScore)
                    battleUIManager.matchResultText.text = "Жаль, но вы проиграли";
                else if (_playerScore > _botScore)
                    battleUIManager.matchResultText.text = "Вы победили!";
                else
                    battleUIManager.matchResultText.text = "Ничья!";
            }
            battleUIManager.matchResultPanel.SetActive(true);
            isGameStart = false;
            StartCoroutine(BackToMenu());
        }
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(3);
        battleUIManager.backBtn.onClick.Invoke();
    }

    public void CheckScore(int score, bool whom)
    {
        if (whom)
        {
            _playerScore += score;
        }
        else
        {
            _botScore += score;
        }
        battleUIManager.scoreText.text = $"{_playerScore} | {_botScore}";
    }

    public void SetHealthBar(int value)
    {
        battleUIManager.healthBar.value = value;
    }
}
