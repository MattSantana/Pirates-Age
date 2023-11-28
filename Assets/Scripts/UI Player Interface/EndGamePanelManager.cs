using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class EndGamePanelManager : MonoBehaviour
{
    [SerializeField] private Button playagainBtn;
    [SerializeField] private Button backToMenuBtn;
    [SerializeField] private AudioSource clickSound;
    void Awake()
    {
        playagainBtn.onClick.AddListener(PlayAgain);
        backToMenuBtn.onClick.AddListener(BackToMenu);       
    }

    private void BackToMenu()
    {
        StartCoroutine(DelayToStartNewScene(0));
        clickSound.Play();
    }

    private void PlayAgain()
    {
        StartCoroutine(DelayToStartNewScene(1));
        clickSound.Play();
    }
    IEnumerator DelayToStartNewScene(int sceneIndex)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
    }
}
