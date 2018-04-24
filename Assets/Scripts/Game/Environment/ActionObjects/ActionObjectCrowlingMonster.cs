using UnityEngine;

public class ActionObjectCrowlingMonster : ActionObject
{
    [SerializeField] private float _timeCrowle = 6.0f;
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private AudioClip _runClip;
    [SerializeField] private AudioSource _audioSource;
    private bool _needMove = false;

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _audioSource.PlayOneShot(_runClip);
        _needMove = true;
    }

    public override void Update()
    {
        base.Update();

        if (_needMove)
        {
            _timeCrowle -= Time.deltaTime;
            if (_timeCrowle <= 0.0f)
            {
                _needMove = false;
            }

            transform.position += transform.forward * Time.deltaTime * _moveSpeed;
        }
    }

}
