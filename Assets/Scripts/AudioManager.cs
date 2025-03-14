using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioPlayed;

    private Coroutine playSoundCoroutine;
    private bool isPlaying;

    public IEnumerator PlayRandomSound()
    {
        if (isPlaying) yield break;  // Prevent multiple starts

        isPlaying = true;

        // Randomizes pitch of the audio for the variable effect
        while (isPlaying)
        {
            audioPlayed.pitch = Random.Range(0.95f, 1.05f);

            audioPlayed.Play();

            yield return new WaitUntil(() => !audioPlayed.isPlaying);
        }

        isPlaying = false;  // Reset when finished
    }

    public void PlayRandomSoundOnce()
    {
        audioPlayed.pitch = Random.Range(0.95f, 1.05f);
        audioPlayed.Play();
    }

    public IEnumerator PlayRandomSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomSoundOnce();
    }

    public void StartRandomSound()
    {
        if (playSoundCoroutine == null)
        {
            playSoundCoroutine = StartCoroutine(PlayRandomSound());
        }
    }

    public void StopRandomSound()
    {
        if (playSoundCoroutine != null)
        {
            StopCoroutine(playSoundCoroutine);
            playSoundCoroutine = null;
        }

        if (audioPlayed.isPlaying)
        {
            audioPlayed.Stop();
        }

        isPlaying = false;
    }


}
