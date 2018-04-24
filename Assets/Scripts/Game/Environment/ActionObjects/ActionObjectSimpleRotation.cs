using UnityEngine;

public class ActionObjectSimpleRotation : ActionObject
{
    [SerializeField] private Transform _rotPivot;
    [Space]
    [SerializeField] private Vector3 _AddLocalEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
    [SerializeField] private float _rotationSpeedScaler = 1.0f;
    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [Space]
    [SerializeField] private AnimationCurve _animationCurve;

    private float _rotationProcess = 2.0f; // >= 1.0f on start
    private Vector3 _prevLocalEulerAngles;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);
        ActivateRotation();
    }

    private void ActivateRotation()
    {
        _prevLocalEulerAngles = _rotPivot.localEulerAngles;
        _rotationProcess = 0.0f;

        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
     
    public override void Update()
    {
        base.Update();

        if (_rotationProcess < 1.0f)
        {
            _rotationProcess += Time.deltaTime * _rotationSpeedScaler;
            float clampedProcess = Mathf.Clamp01(_rotationProcess);
            float lerpScale = _animationCurve.Evaluate(clampedProcess);
            _rotPivot.localEulerAngles = Vector3.Lerp(_prevLocalEulerAngles, _prevLocalEulerAngles + _AddLocalEulerAngles, lerpScale);
        }
    }
}
