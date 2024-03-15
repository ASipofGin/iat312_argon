using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
    }

    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true; // Set the audio to loop

            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Play the music if it's not already playing
            }
        }
    }
}
