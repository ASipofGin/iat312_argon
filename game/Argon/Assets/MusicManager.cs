using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevent the music player from being destroyed on load

        // If there's already a music player in the scene, destroy this one to avoid duplicates
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
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
