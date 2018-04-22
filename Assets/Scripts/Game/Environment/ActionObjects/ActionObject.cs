using UnityEngine;

public class ActionObject : MonoBehaviour
{
    private ActionObjectActivator.ET_ACTIVATE_TYPE _activateType = ActionObjectActivator.ET_ACTIVATE_TYPE.SIMPLE;
    private float _timeToWork;

    private void Update()
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
    }

    public virtual void Deactivate()
    {
        if (_activateType == ActionObjectActivator.ET_ACTIVATE_TYPE.WITH_TIMER)
            _activateType = ActionObjectActivator.ET_ACTIVATE_TYPE.SIMPLE;
    }
	
}
