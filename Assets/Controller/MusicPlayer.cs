using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();

        if (GetComponent<AudioListener>() == null)
            gameObject.AddComponent<AudioListener>();
    }
}
