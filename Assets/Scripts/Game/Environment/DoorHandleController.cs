using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    [SerializeField] private Transform _doorHandle;
    [SerializeField] private AnimationCurve _animCurve;
    [Space]
    [SerializeField] private float _minWaveTime = 1.0f;
    [SerializeField] private float _maxWaveTime = 2.0f;
    [Space]
    [SerializeField] private float _maxShakeAngle = 8.0f;
    [Space]
    [SerializeField] private bool _shakeX = false;
    [SerializeField] private bool _shakeY = true;
    [SerializeField] private bool _shakeZ = true;
    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private float _needShakeTime = 0.0f;
    private Vector3 _startLocalEulerAngles;
    private float _waveTime = 0.0f;
    private float _waveCurrent = -1.0f;
    private Vector3 _shakeLocalEuler = new Vector3(15, 15, 15);

    public void SetShakeSeconds(float t)
    {
        _needShakeTime = t;
    }

    private void Start()
    {
        _startLocalEulerAngles = _doorHandle.localEulerAngles;
    }

    private void Update()
    {
        _needShakeTime -= Time.deltaTime;
        _waveCurrent -= Time.deltaTime;

        CheckSoound();

        if (_needShakeTime >= 0.0f)
        {
            if (_waveCurrent <= 0) // new wave
            {
                _waveTime = Random.Range(_minWaveTime, _maxWaveTime);
                _waveCurrent = _waveTime;

                float axisX = _shakeX ? Random.Range(-_maxShakeAngle, _maxShakeAngle) : 0;
                float axisY = _shakeY ? Random.Range(-_maxShakeAngle, _maxShakeAngle) : 0;
                float axisZ = _shakeZ ? Random.Range(-_maxShakeAngle, _maxShakeAngle) : 0;
                _shakeLocalEuler = new Vector3(
                    _startLocalEulerAngles.x + axisX
                    , _startLocalEulerAngles.y + axisY
                    , _startLocalEulerAngles.z + axisZ);
            }
        }

        if (_waveCurrent >= 0)
        {
            float ratio = Mathf.Clamp01(1.0f - (_waveCurrent / _waveTime));
            Vector3 newLocalEulerAngles = Vector3.Lerp(_startLocalEulerAngles, _shakeLocalEuler, _animCurve.Evaluate(ratio));
            _doorHandle.localEulerAngles = newLocalEulerAngles;
        }
    }

    private void CheckSoound()
    {
        if (_needShakeTime >= 0.0f && !_audioSource.isPlaying)
        {
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
        if (_audioSource.isPlaying && _needShakeTime < 0.0f)
        {
            _audioSource.Stop();
        }
    }
}
