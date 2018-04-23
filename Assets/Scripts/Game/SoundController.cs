using TheHeartbeat.Utilities;
using UnityEngine;

public class SoundController : SingletonBehaviour<SoundController>
{
    [SerializeField] private AudioSource _audioSource;

    private bool _needResetMainSound = false;

    private enum ET_MAIN_SOUND_STATE { NORMAL, DOWN, UP }
    private ET_MAIN_SOUND_STATE _mainSoundState = ET_MAIN_SOUND_STATE.UP;

    private float _mainSoundStateVolume = 0.0f;
    private float _soundDownSpeed = 0.2f;
    private float _soundUpSpeed = 0.2f;

    public void OnMainSceneStart()
    {
        if (_audioSource.isPlaying)
        {
            _mainSoundState = ET_MAIN_SOUND_STATE.DOWN;
        }
        else
        {
            _audioSource.Play();
            _mainSoundStateVolume = _audioSource.volume = 0.0f;
        }
    }

    private void Update()
    {
        if (_mainSoundState == ET_MAIN_SOUND_STATE.UP)
        {
            _mainSoundStateVolume += Time.deltaTime * _soundUpSpeed;
            if (_mainSoundStateVolume >= 1.0f)
                _mainSoundState = ET_MAIN_SOUND_STATE.NORMAL;

            _mainSoundStateVolume = Mathf.Clamp01(_mainSoundStateVolume);
            _audioSource.volume = _mainSoundStateVolume;
        }

        if (_mainSoundState == ET_MAIN_SOUND_STATE.DOWN)
        {
            _mainSoundStateVolume -= Time.deltaTime * _soundDownSpeed;
            if (_mainSoundStateVolume <= 0.0f)
            {
                _mainSoundState = ET_MAIN_SOUND_STATE.UP;
            }
            _mainSoundStateVolume = Mathf.Clamp01(_mainSoundStateVolume);
            _audioSource.volume = _mainSoundStateVolume;
        }
    }

}
