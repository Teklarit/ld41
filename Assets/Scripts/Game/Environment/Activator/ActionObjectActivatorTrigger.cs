using UnityEngine;

public class ActionObjectActivatorTrigger : ActionObjectActivator
{
    [Space]
    [SerializeField] private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ENTER Trigger");
        TryActivate();
    }
}
