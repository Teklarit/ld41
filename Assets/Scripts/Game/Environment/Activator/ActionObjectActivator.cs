using UnityEngine;

public class ActionObjectActivator : MonoBehaviour
{
    public enum ET_ACTIVATE_TYPE { SIMPLE, WITH_TIMER }
    public enum ET_ACTIVATE_COUNT { ALWAYS, ONCE, N }

    [SerializeField] private ActionObject _actionObject;
    [Space]
    [SerializeField] private ET_ACTIVATE_TYPE _activate_type = ET_ACTIVATE_TYPE.SIMPLE;
    [SerializeField] private float _timeToWork = 8.0f;
    [Space]
    [SerializeField] private ET_ACTIVATE_COUNT _activate_count = ET_ACTIVATE_COUNT.ONCE;
    [SerializeField] private int _activate_N_count = 1;

    private int _wasActivations = 0;

    private void Start()
    {
        if (_actionObject == null)
            Debug.LogWarning("ActionObjectActivator: _actionObject is missing!");
    }

    public virtual void TryActivate()
    {
        if (_activate_count == ET_ACTIVATE_COUNT.ALWAYS
            || (_activate_count == ET_ACTIVATE_COUNT.ONCE && _wasActivations == 0)
            || (_activate_count == ET_ACTIVATE_COUNT.N && _wasActivations < _activate_N_count)
            )
        {
            ++_wasActivations;
            _actionObject.Activate(_activate_type, _timeToWork);
        }
    }
}
