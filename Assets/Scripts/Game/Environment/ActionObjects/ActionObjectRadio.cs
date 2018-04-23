using UnityEngine;

public class ActionObjectRadio : ActionObject
{
    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        GetComponent<AudioSource>().Play();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GetComponent<AudioSource>().Stop();
    }
}
