using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankBattle
{
    public class BattleUIManager : MonoBehaviour
    {
        public TMP_Text matchResultText;
        public TMP_Text timeToStartGameText;
        public TMP_Text timeToEndGameText;
        public TMP_Text scoreText;
        public Slider healthBar;
        public GameObject descriptionPanel;
        public GameObject matchResultPanel;
        public Button backBtn;
        public Button exitBtn;
    }
}
