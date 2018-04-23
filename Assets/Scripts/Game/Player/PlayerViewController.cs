﻿using UnityEngine;

public class PlayerViewController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private Transform _view;
    [SerializeField] private float _viewBobMinValue;
    [SerializeField] private float _viewBobMaxValue;
    [SerializeField] private float _sideMovementMult;
    [SerializeField] private float _forwardMovementMult;
    [SerializeField] private float _upMovementMult;
    [SerializeField] private float _viewBobLerpCoef;
    [SerializeField] private float _stepDuration;

    private float sensitivityX = 100.0f;
    private float sensitivityY = 100.0f;

    private float minimumX = 0f;
    private float maximumX = 360f;

    private float minimumY = -30f;
    private float maximumY = 45f;

    private float rotationY = 0f;

    private Vector3 _initialViewLocalPosition;
    private Vector3 _viewOffset;
    private float _idleTimePassed;
    private float _stepTimePassed;

    public Vector3 ViewPosition
    {
        get { return _view.position; }
    }

    public Quaternion ViewRotation
    {
        get { return _view.rotation; }
    }

    public void Init()
    {
        _initialViewLocalPosition = _view.localPosition;
        _idleTimePassed = 0f;
        _stepTimePassed = 0f;
    }

    public void CustomUpdate(float dt)
    {
        var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * dt;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * dt;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.eulerAngles = new Vector3(0f, rotationX, 0f);
        _view.eulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }

    public void CustomLateUpdate(float dt)
    {
        var speedMultiplier = _playerMovementController.SpeedPart;

        if (speedMultiplier <= 0.001f)
        {
            _stepTimePassed = _stepDuration * 0f;
        }

        var clampedMultiplier = Mathf.Lerp(_viewBobMinValue, _viewBobMaxValue, _playerMovementController.SpeedPart);

        _idleTimePassed += clampedMultiplier * dt;
        _stepTimePassed += clampedMultiplier * dt;
        var idlePassedPart = Mathf.Clamp01(_idleTimePassed / _stepDuration);
        var stepPassedPart = Mathf.Clamp01(_stepTimePassed / _stepDuration);

        var idleSideAngle = idlePassedPart * Mathf.PI * 2f;
        var idleForwardAngle = idleSideAngle * 2f;
        var stepSideAngle = stepPassedPart * Mathf.PI * 2f;
        var stepForwardAngle = stepSideAngle * 2f;

        var idleSideOffset = Mathf.Sin(idleSideAngle);
        var idleForwardOffset = Mathf.Sin(idleForwardAngle);
        var stepSideOffset = Mathf.Sin(stepSideAngle);
        var stepForwardOffset = Mathf.Sin(stepForwardAngle);

        var idleViewOffset = new Vector3(idleSideOffset * _sideMovementMult, idleForwardOffset * _upMovementMult, idleForwardOffset * _forwardMovementMult) * clampedMultiplier;
        var stepViewOffset = new Vector3(stepSideOffset * _sideMovementMult, stepForwardOffset * _upMovementMult, stepForwardOffset * _forwardMovementMult) * clampedMultiplier;
        var viewOffset = Vector3.Lerp(idleViewOffset, stepViewOffset, Mathf.Sqrt(speedMultiplier));
        _viewOffset = Vector3.Lerp(_viewOffset, viewOffset, Mathf.Clamp01(_viewBobLerpCoef * dt));

        _view.localPosition = _initialViewLocalPosition + _viewOffset;

        if (_idleTimePassed >= _stepDuration)
        {
            _idleTimePassed -= _stepDuration;
        }

        if (_stepTimePassed >= _stepDuration)
        {
            _stepTimePassed -= _stepDuration;
        }
    }
}
