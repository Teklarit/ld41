using UnityEngine;

public class ActionObjectMetronome : ActionObject
{
    [SerializeField] private Transform _rotPivot;
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _waveTime = 0.5f;
    [SerializeField] private float _maxAngle = 25.0f;
    [Space]
    [SerializeField] private bool _rotateX = true;
    [SerializeField] private bool _rotateY = false;
    [SerializeField] private bool _rotateZ = false;
    [Space]
    [SerializeField] private AudioSource _audioSource;

    private float _needWorkTime = 0.0f;
    public float GetNeedWorkTime() { return _needWorkTime; }

    private float _waveCurrent;
    private Vector3 _shakeLocalEuler;
    private Vector3 _startLocalEulerAngles;

    public virtual void Start()
    {
        _startLocalEulerAngles = _rotPivot.localEulerAngles;
        _shakeLocalEuler = new Vector3(
            _rotateX ? _maxAngle : 0,
            _rotateY ? _maxAngle : 0,
            _rotateZ ? _maxAngle : 0
            );
    }

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _needWorkTime = time;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _needWorkTime = 0.0f;
    }

    public override void Update()
    {
        base.Update();

        _needWorkTime -= Time.deltaTime;
        _waveCurrent -= Time.deltaTime;

        if (_needWorkTime > 0.0f && _waveCurrent <= 0)
            _waveCurrent = _waveTime;

        if (_waveCurrent >= 0)
        {
            float ratio = Mathf.Clamp01(1.0f - (_waveCurrent / _waveTime));
            Vector3 newLocalEulerAngles = _startLocalEulerAngles + (_shakeLocalEuler * _animCurve.Evaluate(ratio));
            _rotPivot.localEulerAngles = newLocalEulerAngles;
        }

        CheckAudio();
    }

    private void CheckAudio()
    {
        if (_needWorkTime > 0 && !_audioSource.isPlaying)
            _audioSource.Play();

        if (_needWorkTime < 0 && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
