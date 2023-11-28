using Pirates.Core;
using UnityEngine;

public class TimesUp : MonoBehaviour
{
    [SerializeField] private AudioSource finalWhistleAudio;
    public void GameTimesUp()
    {
        GameplayObserver.onFinishingGameSession.Invoke();
    }

    public void FinalWhistle()
    {
        finalWhistleAudio.Play();
        GameplayObserver.onFinalWhistle.Invoke();
    }
}
