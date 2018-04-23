using UnityEngine;

public class ActionObjectSomeSound : ActionObject
{
    [SerializeField] private AudioSource _audioSource;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _audioSource.Play();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _audioSource.Stop();
    }
}
