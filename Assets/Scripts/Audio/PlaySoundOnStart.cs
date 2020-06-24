using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] float delayBeforeSound = 2f;
    [SerializeField] float delayBeforeEventCall = 2f;

    [SerializeField] AudioClip clip;
    [SerializeField] UnityEvent onSoundPlayedEvent;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeSound);

        AudioManager.instance.Play(clip);

        yield return new WaitForSeconds(delayBeforeEventCall);

        StartCoroutine(SceneFader.instance.FadeInCo());
        onSoundPlayedEvent?.Invoke();

    }
}
