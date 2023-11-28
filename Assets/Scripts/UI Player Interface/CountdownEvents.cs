using Pirates.Core;
using UnityEngine;

public class CountdownEvents : MonoBehaviour
{
    [SerializeField] private AudioSource beepSound;
    [SerializeField] private AudioSource finishBeepSound;
    public void PlayBeepSound()
    {
        beepSound.Play();
    }

    public void PlayFinishBeepSound()
    {
        finishBeepSound.Play();
    }

    public void FinishingCountdown()
    {
        GameplayObserver.onGameStartCountdownFinished.Invoke();
    }
    public void StartingCountdown()
    {
        GameplayObserver.onGameStart.Invoke();
    }
}
