using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioPlayed;
    public AudioClip[] clips;
    private Coroutine playSoundCoroutine;
    private bool isPlaying;

    public IEnumerator PlayRandomSound()
    {
        if (isPlaying) yield break;  // Prevent multiple starts

        isPlaying = true;

        while (isPlaying)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip clip = clips[randomIndex];

            audioPlayed.clip = clip;
            audioPlayed.Play();

            yield return new WaitForSeconds(clip.length);
        }

        isPlaying = false;  // Reset when finished
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
