using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public enum State
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

    private Dictionary<Hand, Dictionary<State, int>> _animatorIds;
    private Dictionary<Hand, State> _states;

    public void Init()
    {
        _animatorIds = new Dictionary<Hand, Dictionary<State, int>>();
        for (var i = 0; i < (int)Hand.Count; i++)
        {
            var handIds = new Dictionary<State, int>();
            for (var j = 0; j < (int)State.Count; j++)
            {
                var handName = Enum.GetName(typeof(Hand), (Hand)i);
                var stateName = Enum.GetName(typeof(State), (State)j);
                handIds.Add((State)j, Animator.StringToHash(handName + stateName));
            }
            _animatorIds.Add((Hand)i, handIds);
        }

        _states = new Dictionary<Hand, State>();
        for (var i = 0; i < (int)Hand.Count; i++)
        {
            _states[(Hand)i] = State.EmptyIdle;
        }
    }

    public void SetState(Hand hand, State state)
    {
        _states[hand] = state;
        for (var i = 0; i < (int)State.Count; i++)
        {
            if ((State)i != state)
            {
                _animator.SetBool(_animatorIds[hand][(State)i], false);
            }
        }
        _animator.SetBool(_animatorIds[hand][state], true);
    }

    public State GetState(Hand hand)
    {
        return _states[hand];
    }
}
