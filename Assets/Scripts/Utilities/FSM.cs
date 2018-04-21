using System.Collections.Generic;

namespace TheHeartbeat.Utilities
{
    public class FSM<TS, TT>
    {
        private TS _state;
        private TS _nextState;
        private readonly Dictionary<TS, FSMState> _states;
        private readonly Dictionary<TT, FSMTransition<TS>> _transitions;
        private bool _leaving;

        public FSM(Dictionary<TS, FSMState> states, Dictionary<TT, FSMTransition<TS>> transitions, TS initialState)
        {
            _states = states;
            _transitions = transitions;
            SetState(initialState);
        }

        public void Update(float dt)
        {
            var s = _states[_state];
            s.Update(dt);
            HandleLeaving(s);
        }

        public void FixedUpdate(float dt)
        {
            var s = _states[_state];
            s.FixedUpdate(dt);
            HandleLeaving(s);
        }

        public void LateUpdate(float dt)
        {
            var s = _states[_state];
            s.LateUpdate(dt);
            HandleLeaving(s);
        }

        private void HandleLeaving(FSMState s)
        {
            if (_leaving && s.IsLeaved())
            {
                _leaving = false;
                _state = _nextState;
                _states[_state].Enter();
            }
        }

        public void Transit(TT transition)
        {
            var t = _transitions[transition];
            if (t.IsAvailable(_state))
            {
                var s = _states[_state];
                s.Leave();

                if (s.IsLeaved())
                {
                    _leaving = false;
                    _state = t.To();
                    _states[_state].Enter();
                }
                else
                {
                    _leaving = true;
                    _nextState = t.To();
                }
            }
        }

        public TS CurrentState()
        {
            return _state;
        }

        public void SetState(TS state)
        {
            _state = state;
            _nextState = state;
            _leaving = false;
        }
    }
}
