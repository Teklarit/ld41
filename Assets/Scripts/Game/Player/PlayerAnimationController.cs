using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _enemaAnimator;
    [SerializeField] private Animator _flashlightAnimator;

    public enum HandState
    {
        EmptyIdle,
        EnemaIdle,
        EnemaSqueezed,
        FlashlightIdle,
        FlashlightSqueezed,

        Count
    }

    public enum Hand
    {
        Left,
        Right,

        Count
    }

    public enum EnemaState
    {
        Idle,
        Squeezed,

        Count
    }

    public enum FlashlightState
    {
        Idle,
        Squeezed,

        Count
    }

    private Dictionary<Hand, Dictionary<HandState, int>> _handsAnimatorIds;
    private Dictionary<Hand, HandState> _handsStates;

    private Dictionary<EnemaState, int> _enemaAnimatorIds;
    private Dictionary<FlashlightState, int> _flashlightAnimatorIds;

    public void Init()
    {
        _handsAnimatorIds = new Dictionary<Hand, Dictionary<HandState, int>>();
        for (var i = 0; i < (int)Hand.Count; i++)
        {
            var handIds = new Dictionary<HandState, int>();
            for (var j = 0; j < (int)HandState.Count; j++)
            {
                var handName = Enum.GetName(typeof(Hand), (Hand)i);
                var stateName = Enum.GetName(typeof(HandState), (HandState)j);
                handIds.Add((HandState)j, Animator.StringToHash(handName + stateName));
            }
            _handsAnimatorIds.Add((Hand)i, handIds);
        }

        _handsStates = new Dictionary<Hand, HandState>();
        for (var i = 0; i < (int)Hand.Count; i++)
        {
            _handsStates[(Hand)i] = HandState.EmptyIdle;
        }

        _enemaAnimatorIds = new Dictionary<EnemaState, int>();
        for (var j = 0; j < (int)EnemaState.Count; j++)
        {
            var stateName = Enum.GetName(typeof(EnemaState), (EnemaState)j);
            _enemaAnimatorIds.Add((EnemaState)j, Animator.StringToHash(stateName));
        }

        _flashlightAnimatorIds = new Dictionary<FlashlightState, int>();
        for (var j = 0; j < (int)FlashlightState.Count; j++)
        {
            var stateName = Enum.GetName(typeof(FlashlightState), (FlashlightState)j);
            _flashlightAnimatorIds.Add((FlashlightState)j, Animator.StringToHash(stateName));
        }
    }

    public void SetHandState(Hand hand, HandState handState)
    {
        _handsStates[hand] = handState;
        for (var i = 0; i < (int)HandState.Count; i++)
        {
            if ((HandState)i != handState)
            {
                _playerAnimator.SetBool(_handsAnimatorIds[hand][(HandState)i], false);
            }
        }
        _playerAnimator.SetBool(_handsAnimatorIds[hand][handState], true);
    }

    public HandState GetHandState(Hand hand)
    {
        return _handsStates[hand];
    }

    public void SetEnemaState(EnemaState state)
    {
        for (var i = 0; i < (int)EnemaState.Count; i++)
        {
            if ((EnemaState)i != state)
            {
                _enemaAnimator.SetBool(_enemaAnimatorIds[(EnemaState)i], false);
            }
        }

        _enemaAnimator.SetBool(_enemaAnimatorIds[state], true);
    }

    public void SetFlashlightState(FlashlightState state)
    {
        for (var i = 0; i < (int)FlashlightState.Count; i++)
        {
            if ((FlashlightState)i != state)
            {
                _flashlightAnimator.SetBool(_flashlightAnimatorIds[(FlashlightState)i], false);
            }
        }

        _flashlightAnimator.SetBool(_flashlightAnimatorIds[state], true);
    }
}
