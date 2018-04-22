using UnityEngine;

public class ActionObjectTV : ActionObject
{
    [SerializeField] private GameObject _screen;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _screen.SetActive(true);
    }
}
