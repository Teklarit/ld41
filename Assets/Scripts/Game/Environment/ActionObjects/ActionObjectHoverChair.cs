using System.Collections.Generic;
using UnityEngine;

public class ActionObjectHoverChair : ActionObject
{
    [SerializeField] private Transform _chair;

    private Animation _anim;
    private List<string> _animNames = new List<string>();
    private int _currentAnim = -1;
    private int _nAnims = 0;

    private void Start()
    {
        _anim = GetComponentInChildren<Animation>();
        foreach (AnimationState state in _anim)
        {
            _animNames.Add(state.name);
        }
        _nAnims = _animNames.Count;

        // LOOP LAST
        if (_nAnims > 0)
            (_anim[_animNames[_nAnims - 1]]).wrapMode = WrapMode.Loop;
    }

    public override void Activate(ActionObjectActivator.ET_ACTIVATE_TYPE activateType, float time = 0)
    {
        base.Activate(activateType, time);
        
        _anim.Play(_animNames[0]);
        _currentAnim = 0;
    }

    private void Update()
    {
        if (_currentAnim >= 0)
        {
            if (!_anim.isPlaying)
            {
                _currentAnim = Mathf.Clamp(_currentAnim + 1, 0, _nAnims - 1);
                _anim.Play(_animNames[_currentAnim]);
            }
        }
    }
}
