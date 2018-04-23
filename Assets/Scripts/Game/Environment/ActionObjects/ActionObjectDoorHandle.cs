using UnityEngine;

public class ActionObjectDoorHandle : ActionObject
{
    [SerializeField] private DoorHandleController _doorHandleController;
    [Space]
    [SerializeField] private float _overrideShakeTime = 7.0f;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _doorHandleController.SetShakeSeconds(_overrideShakeTime);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        //GetComponent<DoorHandleController>().SetShakeSeconds(0);
    }

}
