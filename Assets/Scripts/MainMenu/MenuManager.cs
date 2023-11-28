using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject startBtnGameObj;
    [SerializeField] private GameObject optionsBtnGameObj;
    [SerializeField] private AudioSource clickSound;
    void Awake()
    {
        startBtn.onClick.AddListener(StartingGame);
        optionBtn.onClick.AddListener(OpenOptionsMenu);       
    }

    private void StartingGame()
    {
        StartCoroutine(DelayToStartGame());
        clickSound.Play();
    }
    IEnumerator DelayToStartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
    private void OpenOptionsMenu()
    {
        optionsPanel.SetActive(true);
        startBtnGameObj.SetActive(false);
        optionsBtnGameObj.SetActive(false);
        clickSound.Play();
    }

}
