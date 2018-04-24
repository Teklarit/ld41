using UnityEngine;

public class ActionObjectHelicopter : ActionObject
{
    [SerializeField] private AnimationCurve _heightCurve;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _flyDuration;

    private Vector3 _initialPosition;
    private float _timePassed;
    private bool _activated;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _initialPosition = transform.position;
        _timePassed = 0f;
        _activated = true;

        var player = FindObjectOfType<PlayerController>();
        player.SetPlayerWon();
    }

    public override void Update()
    {
        base.Update();

        if (!_activated)
        {
            return;
        }

        _timePassed += Time.deltaTime;

        var passedPart = Mathf.Clamp01(_timePassed / _flyDuration);
        var curvedPart = _heightCurve.Evaluate(passedPart);

        var height = curvedPart * _maxHeight;
        transform.position = _initialPosition + Vector3.up * height;

        GetComponentInChildren<RotationHandler>().SetAngularSpeedMultiplier(1f + _speedCurve.Evaluate(passedPart) * 9f);
    }
}
