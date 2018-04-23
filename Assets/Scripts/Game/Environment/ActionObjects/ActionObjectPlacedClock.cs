using UnityEngine;

public class ActionObjectPlacedClock : ActionObjectMetronome
{
    [Space]
    [SerializeField] private Transform _arrowMinutes;
    [SerializeField] private float _minutesSpeed = 200.0f;
    [SerializeField] private bool _rotateMinuteX = false;
    [SerializeField] private bool _rotateMinuteY = false;
    [SerializeField] private bool _rotateMinuteZ = false;
    [Space]
    [SerializeField] private Transform _arrowHours;
    [SerializeField] private float _hoursSpeed = 100.0f;
    [SerializeField] private bool _rotateHoursX = false;
    [SerializeField] private bool _rotateHoursY = false;
    [SerializeField] private bool _rotateHoursZ = false;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);


    }

    public override void Deactivate()
    {
        base.Deactivate();


    }

    public override void Update()
    {
        base.Update();

        if (GetNeedWorkTime() > 0)
        {
            // Minutes
            Vector3 minutesEuler = _arrowMinutes.localEulerAngles;
            float minutChange = Time.deltaTime * _minutesSpeed;
            if (_rotateMinuteX) { minutesEuler.x += minutChange; }
            if (_rotateMinuteY) { minutesEuler.y += minutChange; }
            if (_rotateMinuteZ) { minutesEuler.z += minutChange; }
            _arrowMinutes.localEulerAngles = minutesEuler;

            // Hours
            Vector3 hoursEuler = _arrowHours.localEulerAngles;
            float hoursChange = Time.deltaTime * _hoursSpeed;
            if (_rotateHoursX) { hoursEuler.x += hoursChange; }
            if (_rotateHoursY) { hoursEuler.y += hoursChange; }
            if (_rotateHoursZ) { hoursEuler.z += hoursChange; }
            _arrowHours.localEulerAngles = hoursEuler;
        }
    }
}
