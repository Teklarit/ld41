using UnityEngine;

public class ActionObject : MonoBehaviour
{
    [SerializeField] private float _fearDistance = 10.0f;
    [SerializeField] private float _fearPower = 1.0f;

    private ActionObjectActivator.ET_ACTIVATE_TYPE _activateType = ActionObjectActivator.ET_ACTIVATE_TYPE.SIMPLE;
    private float _timeToWork;

    public virtual void Update()
    {
        if (_activateType == ActionObjectActivator.ET_ACTIVATE_TYPE.WITH_TIMER)
        {
            _timeToWork -= Time.deltaTime;
            if (_timeToWork <= 0)
                Deactivate();
        }
    }

    public virtual void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        if (activateType == ActionObjectActivator.ET_ACTIVATE_TYPE.WITH_TIMER)
        {
            _activateType = activateType;
            _timeToWork = time;
        }
        ApplyFear();
    }

    public virtual void Deactivate()
    {
        if (_activateType == ActionObjectActivator.ET_ACTIVATE_TYPE.WITH_TIMER)
            _activateType = ActionObjectActivator.ET_ACTIVATE_TYPE.SIMPLE;
    }

    public virtual void ApplyFear()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
            return;

        var distance = (playerController.transform.position - transform.position).magnitude;
        if (distance <= _fearDistance)
        {
            // SEND FEAR?
        }
    }
	
}
