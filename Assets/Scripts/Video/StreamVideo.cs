using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage))]
public class StreamVideo : MonoBehaviour
{
    RawImage targetImage;
    [SerializeField] VideoPlayer videoPlayer;

    [Space(10)]

    [SerializeField] bool loop = true;
    public bool isReady = false;
    [SerializeField] Rect videoResolution;
    [SerializeField] UnityEvent onVideoEnded;

#if UNITY_EDITOR

    private void OnValidate()
    {
        if(TryGetComponent(out targetImage))
            targetImage.uvRect = videoResolution;
    }

#endif

    public void Start()
    {
        StartStreamingVideo();


    }

    public void StartStreamingVideo()
    {
        StartCoroutine(PrepareAndPlayVideo());
    }

    public IEnumerator PrepareVideo()
    {
        //isReady nous permet de savoir à tout moment quand la vidéo est prête à être lancée
        isReady = false;

        //Apparemment on doit impérativement arrêter le lecteur pour qu'il puisse changer de clip
        videoPlayer.Stop();

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        isReady = true;
    }

    public void PlayVideo()
    {
        targetImage = GetComponent<RawImage>();

        targetImage.uvRect = videoResolution;
        targetImage.texture = videoPlayer.texture;
        videoPlayer.isLooping = loop;

        videoPlayer.Play();
    }

    private IEnumerator PrepareAndPlayVideo()
    {
        //isReady nous permet de savoir à tout moment quand la vidéo est prête à être lancée
        isReady = false;

        targetImage = GetComponent<RawImage>();

        //Apparemment on doit impérativement arrêter le lecteur pour qu'il puisse changer de clip
        videoPlayer.Stop();

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        targetImage.uvRect = videoResolution;
        targetImage.texture = videoPlayer.texture;
        videoPlayer.isLooping = loop;

        isReady = true;

        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }


    void EndReached(VideoPlayer vp)
    {
        onVideoEnded?.Invoke();
    }
}
