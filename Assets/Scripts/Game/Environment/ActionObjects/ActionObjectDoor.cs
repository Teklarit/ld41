using UnityEngine;

public class ActionObjectDoor : ActionObject
{
    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        GetComponent<DoorController>().SetDoorTarget(true, DoorController.ET_DOOR_SPEED.SLOW);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GetComponent<DoorController>().SetDoorTarget(false, DoorController.ET_DOOR_SPEED.FAST);
    }
}
