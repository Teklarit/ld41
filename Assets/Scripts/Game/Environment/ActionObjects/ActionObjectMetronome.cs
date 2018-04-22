using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObjectMetronome : ActionObject
{
    [SerializeField] private Animation _anim;

    private void Start()
    {
        // Turn on loop
        foreach (AnimationState state in _anim)
            state.wrapMode = WrapMode.Loop;
    }

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _anim.Play();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _anim.Stop();
    }
}
