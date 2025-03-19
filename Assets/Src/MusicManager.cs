using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    private float _volume = .3f;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = _volume;
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }
        audioSource.volume = _volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }
}
