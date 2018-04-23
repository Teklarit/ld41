using UnityEngine;

public class ActionRotatedMan : ActionObjectSimpleRotation
{
    [SerializeField] private Animator _manAnimator;
    [SerializeField] private float _actionAfter = 1.0f;
    private float _actionCurrent = 0.0f;
    bool _needAction = false;


    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);

        _needAction = true;
        _actionCurrent = _actionAfter;
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void Update()
    {
        base.Update();

        if (_needAction)
        {
            _actionCurrent -= Time.deltaTime;
            if (_actionCurrent <= 0)
            {
                _needAction = false;
                CustomAction();
            }
        }
    }

    private void CustomAction()
    {
        _manAnimator.SetTrigger("HeadUp");
    }
}
