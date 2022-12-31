using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public bool isGameStart;
    public RaceUIManager raceUIManager;
    public Timer timer;
    public RaceTankController player;
    public CheckNode playerNode;
    public RoundTrigger roundTrigger;
    public List<CheckNode> tanks;

    private int _playerRacePosititon;

    private void Start()
    {
        playerNode = player.GetComponent<CheckNode>();
        player.raceController = this;
        playerNode.roundTrigger = roundTrigger;

        for(int i = 1; i < tanks.Count; i++)
        {
            tanks[i].raceController = this;
            tanks[i].GetComponent<RaceTankAI>().raceController = this;
            tanks[i].roundTrigger = roundTrigger;
        }
    }

    private void Update()
    {
        CheckStartGame();
        CheckEndGame();
        CheckPlayerPosition();

        tanks = tanks.OrderByDescending(x => x.passedNode).ThenBy(x => x.wayDistance).ToList();
        raceUIManager.racePositionText.text = $"Позиция: {_playerRacePosititon + 1}";
    }

    private void CheckPlayerPosition()
    {
        _playerRacePosititon = tanks.IndexOf(playerNode);
    }

    private void CheckStartGame()
    {
        if (timer.waitForStartGame >= 0)
        {
            raceUIManager.timeToStartGameText.text = $"До начала осталось: {timer.waitForStartGame:f1}";
        }
        else
        {
            raceUIManager.timeToStartGameText.gameObject.SetActive(false);
            raceUIManager.descriptionPanel.SetActive(false);
            isGameStart = true;
        }
    }

    private void CheckEndGame()
    {
        if (timer.waitForEndGame >= 0)
        {
            float minutes = Mathf.FloorToInt(timer.waitForEndGame / 60);
            float seconds = Mathf.FloorToInt(timer.waitForEndGame % 60);
            raceUIManager.raceTimeText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            if (_playerRacePosititon == 0)
                raceUIManager.matchResultText.text = $"Вы победили!\nВаша позиция: {_playerRacePosititon}";
            else
                raceUIManager.matchResultText.text = $"Жаль, но вы проиграли.\nВаша позиция: {_playerRacePosititon}";

            raceUIManager.matchResultPanel.SetActive(true);
            isGameStart = false;
            StartCoroutine(BackToMenu());
        }
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(3);
        raceUIManager.backBtn.onClick.Invoke();
    }
}