using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Pirates.Control;

namespace Pirates.Core
{
    public class GameplayObserver : MonoBehaviour
    {
        [SerializeField] private GameObject spawnersPoints;
        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private GameObject  countdownUiSetup;
        [SerializeField] private GameObject  gameTimeObj;
        [SerializeField] private GameObject  scorePointsObj;
        [SerializeField] private GameObject  gameStatsPanel;
        [SerializeField] private TextMeshProUGUI textScorePoints;
        [SerializeField] private TextMeshProUGUI endGameText;
        [SerializeField] private TextMeshProUGUI finalScorePointsText;
        [SerializeField] private TextMeshProUGUI finalWhistleText;
        [SerializeField] private Image backgroundEndGamePanel;
        private GameObject player;
        private int enemyPointsScore = 0;
        public bool gameHasEnded = false;

        #region start game delegate
        public delegate void OnGameStart();
        public static OnGameStart onGameStart;
        #endregion

        #region end countdown delegate
        public delegate void OnGameStartCountdownFinished();
        public static OnGameStart onGameStartCountdownFinished;
        #endregion

        #region points manager
        public delegate void OnEnemyDeath();
        public static OnEnemyDeath onEnemyDeath;
        #endregion

        #region game finishing manager
        public delegate void OnFinishingGameSession();
        public static OnFinishingGameSession onFinishingGameSession;
        public delegate void OnFinalWhistle();
        public static OnFinishingGameSession onFinalWhistle;

        public delegate void OnPlayerLose();
        public static OnPlayerLose onPlayerLose;
        #endregion

        private void Awake() {
            gameHasEnded = false;
        }
        private void Start() {
            player = GameObject.FindGameObjectWithTag("Player");
            gameHasEnded = false;
        }
        private void UiGameStartSetup()
        {
            gameTimeObj.SetActive(false);
            scorePointsObj.SetActive(false);
            spawnersPoints.SetActive(false);
            player.GetComponent<PlayerController>().enabled = false;
            countdownUiSetup.SetActive(true);
        }

        private void EndCountdownSetup()
        {
            gameTimeObj.SetActive(true);
            scorePointsObj.SetActive(true);
            Destroy(countdownUiSetup,0.3f);
            spawnersPoints.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;  
            backgroundMusic.Play();      
        }

        private void PointsIncrease()
        {
            enemyPointsScore++;

            textScorePoints.text = "Score:" + enemyPointsScore.ToString();
        }

        private void FinalWhistle()
        {
            gameHasEnded = true;
            player.GetComponent<PlayerController>().enabled = false;  

            scorePointsObj.SetActive(false);
            gameTimeObj.SetActive(false);
            spawnersPoints.SetActive(false);
        }
        private void EndGamePanel()
        {
            finalWhistleText.gameObject.SetActive(false);
            gameStatsPanel.SetActive(true);
            finalScorePointsText.text = "Final Score: " + enemyPointsScore.ToString();
            if( player== null )
            {
                endGameText.text = "You Lose!";
                backgroundEndGamePanel.color = new Color(0.698f, 0.384f, 0.227f);

            }
            else{
                endGameText.text = "You Won!";
                backgroundEndGamePanel.color = new Color(0.28f, 0.52f, 0.72f);
            } 
        }

        private void EndGameWhenPlayerLose()
        {
            finalWhistleText.text = "Defeated!";
        }
        private void OnEnable() 
        {
            onGameStart+=UiGameStartSetup;
            onGameStartCountdownFinished+=EndCountdownSetup;
            onEnemyDeath+=PointsIncrease;
            onFinishingGameSession+=EndGamePanel;
            onFinalWhistle+=FinalWhistle;
            onPlayerLose+= EndGameWhenPlayerLose;
        }
        private void OnDisable() 
        {
            onGameStart-=UiGameStartSetup;
            onGameStartCountdownFinished-=EndCountdownSetup;   
            onEnemyDeath-=PointsIncrease;    
            onFinishingGameSession-=EndGamePanel;
            onFinalWhistle-=FinalWhistle;
            onPlayerLose-= EndGameWhenPlayerLose;
        }
        
    }
}

