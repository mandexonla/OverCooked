using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float _warningSoundTimer;
    private bool _playingWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float _burnShowProgressAmount = .5f;
        _playingWarningSound = stoveCounter.IsFried() && e._progressNormalized >= _burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (_playingWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0f)
            {
                float _warningSoundTimerMax = .2f;
                _warningSoundTimer = _warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
