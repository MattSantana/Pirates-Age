using UnityEngine.UI;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Button oneMinutesBtn;
    [SerializeField] private Button twoMinutesBtn;
    [SerializeField] private Button threeMinutesBtn;
    [SerializeField] private Button fiveSecondsBtn;
    [SerializeField] private Button tenSecondsBtn;
    [SerializeField] private Button fifteenSecondsBtn;
    [SerializeField] private Button backToMainOptionsBtn;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject startBtnGameObj;
    [SerializeField] private GameObject optionsBtnGameObj;
    [SerializeField] private AudioSource clickSound;
    private void Awake() 
    {
        //GameTimeSection
        oneMinutesBtn.onClick.AddListener(OneMinutesButtonClick);
        twoMinutesBtn.onClick.AddListener(TwoMinutesButtonClick);
        threeMinutesBtn.onClick.AddListener(ThreeMinutesButtonClick);

        //Enemy Spawner Time Section
        fiveSecondsBtn.onClick.AddListener(FiveSecondsButtonClick);
        tenSecondsBtn.onClick.AddListener(TenSecondsButtonClick);
        fifteenSecondsBtn.onClick.AddListener(FifteenSecondsButtonClick);

        // back button
        backToMainOptionsBtn.onClick.AddListener(BackButtonClick);
    }
    public void OneMinutesButtonClick()
    {
        PlayerPrefs.SetFloat("GameTime", 60);
        oneMinutesBtn.interactable = false;
        clickSound.Play();
    }
    public void TwoMinutesButtonClick()
    {
        PlayerPrefs.SetFloat("GameTime", 120);
        twoMinutesBtn.interactable = false;
        clickSound.Play();
    }
    public void ThreeMinutesButtonClick()
    {
        PlayerPrefs.SetFloat("GameTime", 180);
        threeMinutesBtn.interactable = false;
        clickSound.Play();
    }
    public void FiveSecondsButtonClick()
    {
        PlayerPrefs.SetFloat("SpawnerTime", 5);
        fiveSecondsBtn.interactable = false;
        clickSound.Play();
    }
    public void TenSecondsButtonClick()
    {
        PlayerPrefs.SetFloat("SpawnerTime", 10);
        tenSecondsBtn.interactable = false;
        clickSound.Play();
    }

    public void FifteenSecondsButtonClick()
    {
        PlayerPrefs.SetFloat("SpawnerTime", 15);
        fifteenSecondsBtn.interactable = false;
        clickSound.Play();
    }
    public void BackButtonClick()
    {
        optionsPanel.SetActive(false);
        startBtnGameObj.SetActive(true);
        optionsBtnGameObj.SetActive(true);
        clickSound.Play();
    }

}
