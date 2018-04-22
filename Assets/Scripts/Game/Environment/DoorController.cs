using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum ET_DOOR_SPEED { SLOW, FAST }
    private ET_DOOR_SPEED _doorSpeed = ET_DOOR_SPEED.SLOW;

    [SerializeField] private Transform _rotationPivot;
    [SerializeField] private float _maxOpenAngle = 130.0f;
    [SerializeField] private AnimationCurve _universalCurve;
    [Space]
    [SerializeField] private float _slowTime = 0.4f;
    [SerializeField] private float _fastTime = 2.0f;
    
    private float _doorChangeProcess = 0.0f;
    private float _startAngle = 0.0f;
    private float _targetAngle = 0.0f;
    private bool _isTargetOpen = false;
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetDoorTarget(true, ET_DOOR_SPEED.SLOW);
            GetComponent<DoorHandleController>().SetShakeSeconds(20.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SetDoorTarget(false, ET_DOOR_SPEED.SLOW); }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { SetDoorTarget(true, ET_DOOR_SPEED.FAST); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SetDoorTarget(false, ET_DOOR_SPEED.FAST); }

        if (_doorChangeProcess < 1.0f)
        {
            float dt = Time.deltaTime;
            if (_doorSpeed == ET_DOOR_SPEED.SLOW) { dt *= _slowTime; }
            if (_doorSpeed == ET_DOOR_SPEED.FAST) { dt *= _fastTime; }

            _doorChangeProcess += dt;
            float clampedChangeProcess = Mathf.Clamp01(_doorChangeProcess);
            clampedChangeProcess = _universalCurve.Evaluate(clampedChangeProcess);

            Vector3 rotPivotEuler = _rotationPivot.localRotation.eulerAngles;
            rotPivotEuler.y = Mathf.Lerp(_startAngle, _targetAngle, clampedChangeProcess);
            _rotationPivot.localEulerAngles = rotPivotEuler;
        }
    }

    public void SetDoorTarget(bool isOpen, ET_DOOR_SPEED doorSpeed)
    {
        if (isOpen == _isTargetOpen)
        {
            if (doorSpeed == _doorSpeed)
                return;
            _doorSpeed = doorSpeed;
        }

        _doorSpeed = doorSpeed;
        _doorChangeProcess = 0.0f;

        _isTargetOpen = isOpen;
        _targetAngle = isOpen ? _maxOpenAngle : 0.0f;
        _startAngle = _rotationPivot.localRotation.eulerAngles.y;
    }

}
