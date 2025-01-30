using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioPlayed;
    public AudioClip[] clips;

    public bool variableVelocity = false;
    public float velocityMultiplier;

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

            yield return new WaitUntil(() => !audioPlayed.isPlaying);
        }

        isPlaying = false;  // Reset when finished
    }

    public void Update()
    {
        if (variableVelocity)
        {
            ChangePitch();
        }
    }

    public void ChangePitch()
    {
        audioPlayed.pitch = velocityMultiplier;
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
