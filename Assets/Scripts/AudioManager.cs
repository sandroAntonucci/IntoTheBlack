using System.Collections;
using UnityEngine;

// Can't be a static class because it needs to preserve variables about the audio source
public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;

    public bool isFadingOut = false;
    public bool isFadingIn = false;

    public void FadeOut(float fadeDuration, bool stopsAudio)
    {
        if (audioSource.isPlaying == false)
        {
            return;
        }
        StartCoroutine(FadeOutCoroutine(audioSource, fadeDuration, stopsAudio));
    }

    public void FadeIn(float fadeDuration = 0.5f)
    {
        if (audioSource.isPlaying == true)
        {
            return;
        }
        StartCoroutine(FadeInCoroutine(audioSource, fadeDuration));
    }
    
    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeDuration, bool stopsAudio)
    {
        float startVolume = audioSource.volume;

        isFadingIn = false;
        isFadingOut = true;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        isFadingOut = false;

        if (stopsAudio) audioSource.Stop();
        else audioSource.Pause();

        audioSource.volume = startVolume; // Reset volume if needed
    }

    private IEnumerator FadeInCoroutine(AudioSource audioSource, float fadeDuration)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        isFadingOut = false;
        isFadingIn = true;

        float targetVolume = 1f; // You can change this to a desired max volume

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        isFadingIn = false;

        audioSource.volume = targetVolume; // Ensure volume is set exactly to the target
    }
}
