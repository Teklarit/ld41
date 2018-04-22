using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObjectHoverChair : ActionObject
{
    [SerializeField] private Transform _chair;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _chair.transform.position += new Vector3(0, 1, 0);
    }
}
