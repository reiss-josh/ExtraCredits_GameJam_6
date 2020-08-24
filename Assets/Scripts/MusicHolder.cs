//https://answers.unity.com/questions/1260393/make-music-continue-playing-through-scenes.html
using UnityEngine;

public class MusicHolder : MonoBehaviour
{
    private AudioSource _audioSource;
    private static MusicHolder thisInstance;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

        if (thisInstance == null)
        {
            thisInstance = this; PlayMusic();
        }
        else Destroy(gameObject);
        
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}