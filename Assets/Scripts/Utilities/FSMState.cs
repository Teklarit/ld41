using System.Collections.Generic;

namespace TheHeartbeat.Utilities
{
    public class FSMState
    {
        public enum Updatables
        {
            OnEnter = 1,
            OnUpdate = 3,
            OnLeave = 6
        }

        public enum Immediates
        {
            OnPreEnter = 0,
            OnPostEnter = 2,
            OnPostUpdate = 4,
            OnPostLeave = 7
        }

        private const int Limbo = 5;
        private const int Leaved = 8;

        public delegate bool Updatable(float dt);

        public delegate void Immediate();

        private readonly Dictionary<Immediates, Immediate> _immediates;
        private readonly Dictionary<Updatables, Updatable> _updatables;
        private readonly Dictionary<Updatables, Updatable> _fixedUpdatables;
        private readonly Dictionary<Updatables, Updatable> _lateUpdatables;

        private readonly bool _immediatesNotNull;
        private readonly bool _updatablesNotNull;
        private readonly bool _fixedUpdatablesNotNull;
        private readonly bool _lateUpdatablesNotNull;

        private bool _onEnterAvailable;
        private bool _onUpdateAvailable;
        private bool _onLeaveAvailable;

        private int _state;

        public FSMState(Dictionary<Immediates, Immediate> immediates, Dictionary<Updatables, Updatable> updatables,
            Dictionary<Updatables, Updatable> fixedUpdatables, Dictionary<Updatables, Updatable> lateUpdatables)
        {
            _immediates = immediates;
            _updatables = updatables;
            _fixedUpdatables = fixedUpdatables;
            _lateUpdatables = lateUpdatables;

            _immediatesNotNull = immediates != null;
            _updatablesNotNull = updatables != null;
            _fixedUpdatablesNotNull = fixedUpdatables != null;
            _lateUpdatablesNotNull = lateUpdatables != null;

            Reset();
        }

        public FSMState(Dictionary<Immediates, Immediate> immediates, Dictionary<Updatables, Updatable> updatables,
            Dictionary<Updatables, Updatable> fixedUpdatables)
        {
            _immediates = immediates;
            _updatables = updatables;
            _fixedUpdatables = fixedUpdatables;

            _immediatesNotNull = immediates != null;
            _updatablesNotNull = updatables != null;
            _fixedUpdatablesNotNull = fixedUpdatables != null;
            _lateUpdatablesNotNull = false;

            Reset();
        }

        public FSMState(Dictionary<Immediates, Immediate> immediates, Dictionary<Updatables, Updatable> updatables)
        {
            _immediates = immediates;
            _updatables = updatables;

            _immediatesNotNull = immediates != null;
            _updatablesNotNull = updatables != null;
            _fixedUpdatablesNotNull = false;
            _lateUpdatablesNotNull = false;

            Reset();
        }

        public FSMState(Dictionary<Immediates, Immediate> immediates)
        {
            _immediates = immediates;

            _immediatesNotNull = immediates != null;
            _updatablesNotNull = false;
            _fixedUpdatablesNotNull = false;
            _lateUpdatablesNotNull = false;

            Reset();
        }

        public FSMState(Dictionary<Updatables, Updatable> updatables)
        {
            _updatables = updatables;

            _immediatesNotNull = false;
            _updatablesNotNull = updatables != null;
            _fixedUpdatablesNotNull = false;
            _lateUpdatablesNotNull = false;

            Reset();
        }

        public FSMState()
        {
            _immediatesNotNull = false;
            _updatablesNotNull = false;
            _fixedUpdatablesNotNull = false;
            _lateUpdatablesNotNull = false;

            Reset();
        }

        private void Reset()
        {
            _onEnterAvailable = (_updatablesNotNull && _updatables.ContainsKey(Updatables.OnEnter)) ||
                                (_fixedUpdatablesNotNull && _fixedUpdatables.ContainsKey(Updatables.OnEnter)) ||
                                (_lateUpdatablesNotNull && _lateUpdatables.ContainsKey(Updatables.OnEnter));

            _onUpdateAvailable = (_updatablesNotNull && _updatables.ContainsKey(Updatables.OnUpdate)) ||
                                 (_fixedUpdatablesNotNull && _fixedUpdatables.ContainsKey(Updatables.OnUpdate)) ||
                                 (_lateUpdatablesNotNull && _lateUpdatables.ContainsKey(Updatables.OnUpdate));

            _onLeaveAvailable = (_updatablesNotNull && _updatables.ContainsKey(Updatables.OnLeave)) ||
                                (_fixedUpdatablesNotNull && _fixedUpdatables.ContainsKey(Updatables.OnLeave)) ||
                                (_lateUpdatablesNotNull && _lateUpdatables.ContainsKey(Updatables.OnLeave));

            _state = _onUpdateAvailable ? (int) Updatables.OnUpdate : Limbo;
        }

        public void Update(float dt)
        {
            var state = (Updatables) _state;
            if (_updatablesNotNull && _updatables.ContainsKey(state) && _updatables[state](dt))
            {
                HandleStateInc();
            }
        }

        public void FixedUpdate(float dt)
        {
            var state = (Updatables) _state;
            if (_fixedUpdatablesNotNull && _fixedUpdatables.ContainsKey(state) && _fixedUpdatables[state](dt))
            {
                HandleStateInc();
            }
        }

        public void LateUpdate(float dt)
        {
            var state = (Updatables) _state;
            if (_lateUpdatablesNotNull && _lateUpdatables.ContainsKey(state) && _lateUpdatables[state](dt))
            {
                HandleStateInc();
            }
        }

        private void HandleStateInc()
        {
            var postKey = _state + 1;
            var state = (Immediates) postKey;
            if (_immediatesNotNull && _immediates.ContainsKey(state))
            {
                _state = postKey;
                _immediates[state]();
            }

            _state = postKey + 1;
        }

        public void Enter()
        {
            if (_immediatesNotNull)
            {
                _state = (int) Immediates.OnPreEnter;

                if (_immediates.ContainsKey(Immediates.OnPreEnter))
                {
                    _immediates[Immediates.OnPreEnter]();
                }

                if (_state == (int) Immediates.OnPreEnter)
                {
                    if (!_onEnterAvailable)
                    {
                        _state = (int) Immediates.OnPostEnter;

                        if (_immediates.ContainsKey(Immediates.OnPostEnter))
                        {
                            _immediates[Immediates.OnPostEnter]();
                        }
                    }
                    else
                    {
                        _state = (int) Updatables.OnEnter;
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (_state == (int) Immediates.OnPostEnter)
                {
                    if (!_onUpdateAvailable)
                    {
                        if (_immediates.ContainsKey(Immediates.OnPostUpdate))
                        {
                            _state = (int) Immediates.OnPostUpdate;

                            _immediates[Immediates.OnPostUpdate]();

                            if (_state != (int) Immediates.OnPostUpdate)
                            {
                                return;
                            }
                        }

                        _state = Limbo;
                    }
                    else
                    {
                        _state = (int) Updatables.OnUpdate;
                    }
                }

            }
            else
            {
                _state = _onEnterAvailable
                    ? (int) Updatables.OnEnter
                    : (_onUpdateAvailable ? (int) Updatables.OnUpdate : Limbo);
            }
        }

        public void Leave()
        {
            if (_immediatesNotNull)
            {
                if (_state < (int) Immediates.OnPostEnter && _immediates.ContainsKey(Immediates.OnPostEnter))
                {
                    _state = (int) Immediates.OnPostEnter;
                    _immediates[Immediates.OnPostEnter]();

                    if (_state != (int) Immediates.OnPostEnter)
                    {
                        return;
                    }
                }

                if (_state < (int) Immediates.OnPostUpdate && _immediates.ContainsKey(Immediates.OnPostUpdate))
                {
                    _state = (int) Immediates.OnPostUpdate;
                    _immediates[Immediates.OnPostUpdate]();

                    if (_state != (int) Immediates.OnPostUpdate)
                    {
                        return;
                    }
                }

                if (_state < (int) Updatables.OnLeave && _onLeaveAvailable)
                {
                    _state = (int) Updatables.OnLeave;
                    return;
                }

                if (_state < (int) Immediates.OnPostLeave && _immediates.ContainsKey(Immediates.OnPostLeave))
                {
                    _state = (int) Immediates.OnPostLeave;
                    _immediates[Immediates.OnPostLeave]();

                    if (_state != (int) Immediates.OnPostLeave)
                    {
                        return;
                    }
                }

            }
            else if (_state < (int) Updatables.OnLeave && _onLeaveAvailable)
            {
                _state = (int) Updatables.OnLeave;
                return;
            }

            _state = Leaved;
        }

        public bool IsLeaved()
        {
            return _state == Leaved;
        }
    }
}
