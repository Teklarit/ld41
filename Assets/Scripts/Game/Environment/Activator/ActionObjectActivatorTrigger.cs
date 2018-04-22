using UnityEngine;

public class ActionObjectActivatorTrigger : ActionObjectActivator
{
    [Space]
    [SerializeField] private Collider _collider;
    [Space]
    [SerializeField] private LayerMask _layerMaskFilter;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TriggerEnter");
        if ((_layerMaskFilter.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            //Debug.Log("Trigger good mask!");
            TryActivate();
        }
    }
}
