using System.Collections;
using System.Linq;
using UnityEngine;

namespace Sumo
{
    public class SumoController : MonoBehaviour
    {
        public bool isWin;
        public bool isRoundOver;
        public bool isGameStart;
        public SumoTankController player;
        public SumoTankAI bot;
        public SumoUIManager sumoUIManager;
        public Timer timer;
        public PathAI path;
        public GameObject playerPrefab;
        public GameObject botPrefab;

        private int _playerRoundWins = 0;
        private int _botRoundWins = 0;
        private int _roundCounter = 1;
        private Vector3 _playerStartPosition;
        private Vector3 _botStartPosition;
        private Quaternion _playerStartRotation;
        private Quaternion _botStartRotation;

        private void Start()
        {
            _playerStartPosition = player.transform.position;
            _playerStartRotation = player.transform.rotation;
            _botStartPosition = bot.transform.position;
            _botStartRotation = bot.transform.rotation;

            player.GetComponent<SumoTankController>();
            player.sumoController = this;
            player.GetComponentInChildren<SumoTankGun>().sumoController = this;
            bot.GetComponent<SumoTankAI>();
            bot.sumoController = this;
            bot.GetComponentInChildren<SumoTankAIGun>().sumoController = this;
            bot.GetComponentInChildren<SumoTankAIGun>().path = path.nodes;
            bot.nodes = path.nodes;
            sumoUIManager.timeToStartGameText.gameObject.SetActive(true);
            sumoUIManager.scoreText.text = $"{_playerRoundWins} | {_botRoundWins}";
        }

        private void Update()
        {
            CheckStartGame();
            CheckEndGame();
        }

        public void CheckMatchResult()
        {
            if (!isRoundOver)
            {
                if (isWin)
                {
                    _playerRoundWins++;
                    StartCoroutine(RestartRound());
                }
                else
                {
                    _botRoundWins++;
                    StartCoroutine(RestartRound());
                }
            }
        }

        private void CheckStartGame()
        {
            if (timer.waitForStartGame >= 0)
            {
                sumoUIManager.timeToStartGameText.text = $"До начала осталось: {timer.waitForStartGame:f1}";
            }
            else
            {
                sumoUIManager.timeToStartGameText.gameObject.SetActive(false);
                sumoUIManager.descriptionPanel.SetActive(false);
                isGameStart = true;
            }
        }

        private void CheckEndGame()
        {
            if (timer.waitForEndGame >= 0)
            {
                float minutes = Mathf.FloorToInt(timer.waitForEndGame / 60);
                float seconds = Mathf.FloorToInt(timer.waitForEndGame % 60);
                sumoUIManager.timeToEndGameText.text = $"{minutes:00}:{seconds:00}";
            }
            else
            {
                if (_botRoundWins > _playerRoundWins)
                    sumoUIManager.matchResultText.text = $"Жаль, но вы проиграли.\nСчет {_playerRoundWins} | {_botRoundWins}";
                else if (_playerRoundWins > _botRoundWins)
                    sumoUIManager.matchResultText.text = $"Вы победили!\nСчет {_playerRoundWins} | {_botRoundWins}";
                else
                    sumoUIManager.matchResultText.text = $"Ничья!\nСчет {_playerRoundWins} | {_botRoundWins}";

                sumoUIManager.matchResultPanel.SetActive(true);
                isGameStart = false;
                StartCoroutine(BackToMenu());
            }
        }

        private IEnumerator BackToMenu()
        {
            yield return new WaitForSeconds(3);
            sumoUIManager.backBtn.onClick.Invoke();
        }

        private IEnumerator RestartRound()
        {
            yield return new WaitForSeconds(3);
            isWin = false;
            isRoundOver = false;

            DestroyImmediate(player.gameObject);
            player = Instantiate(playerPrefab, _playerStartPosition, _playerStartRotation, path.transform).GetComponent<SumoTankController>();
            player.sumoController = this;
            player.GetComponentInChildren<SumoTankGun>().sumoController = this;

            path.nodes = path.GetComponentsInChildren<Transform>().ToList();
            path.nodes.RemoveAt(0);

            Destroy(bot.gameObject);
            bot = Instantiate(botPrefab, _botStartPosition, _botStartRotation).GetComponent<SumoTankAI>();
            bot.sumoController = this;
            bot.GetComponentInChildren<SumoTankAIGun>().sumoController = this;
            bot.GetComponentInChildren<SumoTankAIGun>().path = path.nodes;
            bot.nodes = path.nodes;

            _roundCounter++;
            sumoUIManager.roundsText.text = $"Раунд {_roundCounter}";
            sumoUIManager.scoreText.text = $"{_playerRoundWins} | {_botRoundWins}";
            player.isGround = true;
            bot.isGround = true;
        }
    }
}
