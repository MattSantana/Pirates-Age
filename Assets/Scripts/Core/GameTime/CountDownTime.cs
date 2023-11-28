using UnityEngine;
using TMPro;
using Pirates.Core;
using Pirates.Control;

public class CountDownTime : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private float startingTime = 120f;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject timesUpObj;
    [SerializeField] private AudioSource backGroundMusic;
    private GameObject player;
    
    private void Awake() {
        startingTime = PlayerPrefs.GetFloat("GameTime");
    }
    void Start()
    {
        currentTime = startingTime;
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateCountDownText();
    }

    private void Update()
    {
        CountDown();
        UpdateCountDownText();

        if (currentTime <= 0)
        {
            timesUpObj.SetActive(true);
            backGroundMusic.Stop();
        }
        else if(player.GetComponent<HealthPlayer>().IsDead())
        {
            timesUpObj.SetActive(true);
            backGroundMusic.Stop();
            GameplayObserver.onPlayerLose.Invoke();
        }
    }

    public void CountDown()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, startingTime);
    }

    void UpdateCountDownText()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
